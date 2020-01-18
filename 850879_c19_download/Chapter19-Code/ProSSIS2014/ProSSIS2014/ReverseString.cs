#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
#endregion  

//    , IconResource = "Wrox.Pipeline.ReverseString.ico")]
namespace Wrox.Pipeline
{
    [DtsPipelineComponent(
 DisplayName = "Wrox Reverse String",
 UITypeName = "Wrox.Pipeline.UI.ReverseStringUI, Wrox.Pipeline.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6aa87e6360f8b842",
 ComponentType = ComponentType.Transform)]

    public class ReverseString : PipelineComponent
    {

        private ColumnInfo[] _inputColumnInfos;
        const string ErrorInvalidUsageType = "Invalid UsageType for column '{0}'";
        const string ErrorInvalidDataType = "Invalid DataType for column '{0}'";

        [CLSCompliant(false)]
        public struct ColumnInfo
        {
            public int bufferColumnIndex;
            public DTSRowDisposition columnDisposition;
            public int lineageID;
        }

        public override void ProvideComponentProperties()
        {
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();
            RemoveAllInputsOutputsAndCustomProperties();

            ComponentMetaData.UsesDispositions = true;

            IDTSInput100 ReverseStringInput = ComponentMetaData.InputCollection.New();
            ReverseStringInput.Name = "RSin";

            ReverseStringInput.ErrorRowDisposition = DTSRowDisposition.RD_FailComponent;

            IDTSOutput100 ReverseStringOutput = ComponentMetaData.OutputCollection.New();
            ReverseStringOutput.Name = "RSout";

            ReverseStringOutput.SynchronousInputID = ReverseStringInput.ID;
            ReverseStringOutput.ExclusionGroup = 1;
            AddErrorOutput("RSErrors", ReverseStringInput.ID, ReverseStringOutput.ExclusionGroup);

            // Connections are not actually needed for this component
            // They are used simply to show how to surface them in the UI
            IDTSRuntimeConnection100 rtc = ComponentMetaData.RuntimeConnectionCollection.New();
            rtc.Name = "Connection";
        }

        [CLSCompliant(false)]
        public override DTSValidationStatus Validate()
        {
            bool Cancel;
            if (ComponentMetaData.AreInputColumnsValid == false)
                return DTSValidationStatus.VS_NEEDSNEWMETADATA;

            foreach (IDTSInputColumn100 inputColumn in ComponentMetaData.InputCollection[0].InputColumnCollection)
            {
                if (inputColumn.UsageType != DTSUsageType.UT_READWRITE)
                {

                    ComponentMetaData.FireError(0, inputColumn.IdentificationString, String.Format(ErrorInvalidUsageType, inputColumn.Name), "", 0, out Cancel);
                    return DTSValidationStatus.VS_ISBROKEN;
                }

                if (inputColumn.DataType != DataType.DT_STR && inputColumn.DataType != DataType.DT_WSTR)
                {

                    ComponentMetaData.FireError(0, inputColumn.IdentificationString, String.Format(ErrorInvalidDataType, inputColumn.Name), "", 0, out Cancel);
                    return DTSValidationStatus.VS_ISBROKEN;
                }

            }
            return base.Validate();
        }

        [CLSCompliant(false)]
        public override IDTSInputColumn100 SetUsageType(int inputID, IDTSVirtualInput100 virtualInput, int lineageID, DTSUsageType usageType)
        {
            IDTSVirtualInputColumn100 virtualInputColumn = virtualInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(lineageID);
            if (usageType == DTSUsageType.UT_READONLY)
                throw new Exception(String.Format(ErrorInvalidUsageType, virtualInputColumn.Name));

            if (usageType == DTSUsageType.UT_READWRITE)
            {
                if (virtualInputColumn.DataType != DataType.DT_STR && virtualInputColumn.DataType != DataType.DT_WSTR)
                {
                    throw new Exception(String.Format(ErrorInvalidDataType, virtualInputColumn.Name));
                }
            }

            return base.SetUsageType(inputID, virtualInput, lineageID, usageType);
        }

        [CLSCompliant(false)]
        public override IDTSOutput100 InsertOutput(DTSInsertPlacement insertPlacement, int outputID)
        {
            throw new Exception("You cannot insert an output (" + outputID.ToString() + ")");
        }

        [CLSCompliant(false)]
        public override IDTSInput100 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            throw new Exception("You cannot insert an input  (" + inputID.ToString() + ")");
        }

        [CLSCompliant(false)]
        public override void DeleteInput(int inputID)
        {
            throw new Exception("You cannot delete an input");
        }

        [CLSCompliant(false)]
        public override void DeleteOutput(int outputID)
        {
            throw new Exception("You cannot delete an ouput");
        }

        public override void PreExecute()
        {
            // Prepare array of column information. Processing requires
            // lineageID so we can do this once in advance.
            IDTSInput100 input = ComponentMetaData.InputCollection[0];

            _inputColumnInfos = new ColumnInfo[input.InputColumnCollection.Count];
            for (int x = 0; x < input.InputColumnCollection.Count; x++)
            {
                IDTSInputColumn100 column = input.InputColumnCollection[x];
                _inputColumnInfos[x] = new ColumnInfo();
                _inputColumnInfos[x].bufferColumnIndex = BufferManager.FindColumnByLineageID(input.Buffer, column.LineageID);

                _inputColumnInfos[x].columnDisposition = column.ErrorRowDisposition;
                _inputColumnInfos[x].lineageID = column.LineageID;
            }

        }


        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {

            int errorOutputID = -1;
            int errorOutputIndex = -1;
            int GoodOutputId = -1;
            IDTSInput100 inp = ComponentMetaData.InputCollection.GetObjectByID(inputID);
            #region Output IDs
            GetErrorOutputInfo(ref errorOutputID, ref errorOutputIndex);
            // There is an error output defined
            errorOutputID = ComponentMetaData.OutputCollection["RSErrors"].ID;
            GoodOutputId = ComponentMetaData.OutputCollection["RSout"].ID;
            #endregion

            while (buffer.NextRow())
            {
                // Check if we have columns to process
                if (_inputColumnInfos.Length == 0)
                {
                    // We do not have to have columns. This is a Sync component so the
                    // rows will flow through regardless. Could expand Validate to check
                    // for columns in the InputColumnCollection
                    buffer.DirectRow(GoodOutputId);
                }
                else
                {
                    try
                    {
                        for (int x = 0; x < _inputColumnInfos.Length; x++)
                        {
                            ColumnInfo columnInfo = _inputColumnInfos[x];
                            if (!buffer.IsNull(columnInfo.bufferColumnIndex))
                            {
                                // Get value as character array
                                char[] chars = buffer.GetString(columnInfo.bufferColumnIndex).ToString().ToCharArray();
                                // Reverse order of characters in array
                                Array.Reverse(chars);
                                // Reassemble reversed value as string
                                string s = new string(chars);
                                // Set output value in buffer
                                buffer.SetString(columnInfo.bufferColumnIndex, s);
                            }
                        }
                        buffer.DirectRow(GoodOutputId);
                    }
                    catch (Exception ex)
                    {
                        switch (inp.ErrorRowDisposition)
                        {
                            case DTSRowDisposition.RD_RedirectRow:
                                buffer.DirectErrorRow(errorOutputID, 0, buffer.CurrentRow);
                                break;

                            case DTSRowDisposition.RD_FailComponent:
                                throw new Exception("Error processing " + ex.Message);
                            case DTSRowDisposition.RD_IgnoreFailure:
                                buffer.DirectRow(GoodOutputId);
                                break;
                        }
                    }
                }
            }
        }
    }
}
