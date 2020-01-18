using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Collections;
using System.IO;
namespace Wrox.Pipeline
{
    [DtsPipelineComponent(
       DisplayName = "Wrox SSIS Destination Adapter",
       ComponentType = ComponentType.DestinationAdapter)]

    public class ProfSSISDestAdapter : PipelineComponent
    {

        #region ColumnInfo
        private struct ColumnInfo
        {
            public int BufferColumnIndex;
            public string ColumnName;
        }
        #endregion

        #region Variables
        private ArrayList _columnInfos = new ArrayList();
        private Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType _fil;
        private string _filename;
        FileStream _fs;
        StreamWriter _sw;
        #endregion

        #region PipelineComponent Methods

        #region Design Time

        public override void ProvideComponentProperties()
        {
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();
            RemoveAllInputsOutputsAndCustomProperties();

            ComponentMetaData.Name = "Wrox SSIS Destination Adapter";
            ComponentMetaData.Description = "Our first Destination Adapter";
            ComponentMetaData.ContactInfo = "www.wrox.com";

            IDTSRuntimeConnection100 rtc = ComponentMetaData.RuntimeConnectionCollection.New();
            rtc.Name = "File To Write";
            rtc.Description = "This is the file to which we want to write";

            IDTSInput100 input = ComponentMetaData.InputCollection.New();
            input.Name = "Component Input";
            input.Description = "This is what we see from the upstream component";
            input.HasSideEffects = true;
        }

        public override void AcquireConnections(object transaction)
        {
            bool pbCancel = false;

            if (ComponentMetaData.RuntimeConnectionCollection["File To Write"].ConnectionManager != null)
            {
                ConnectionManager cm = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(ComponentMetaData.RuntimeConnectionCollection["File To Write"].ConnectionManager);
                if (cm.CreationName != "FILE")
                {
                    ComponentMetaData.FireError(0, "Acquire Connections", "The Connection Manager is not a FILE Connection Manager", "", 0, out pbCancel);
                    throw new Exception("The Connection Manager is not a FILE Connection Manager");
                }
                else
                {
                    _fil = (Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType)cm.Properties["FileUsageType"].GetValue(cm);
                    if (_fil != Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType.CreateFile)
                    {
                        ComponentMetaData.FireError(0, "Acquire Connections", "The type of FILE connection manager must be an Existing File", "", 0, out pbCancel);
                        throw new Exception("The type of FILE connection manager must be Create File");
                    }
                    else
                    {
                        _filename = ReturnConnectionObject(ComponentMetaData.RuntimeConnectionCollection, "File To Write", transaction);
                        if (_filename == null || _filename.Length == 0)
                        {
                            ComponentMetaData.FireError(0, "Acquire Connections", "Nothing returned when grabbing the filename", "", 0, out pbCancel);
                            throw new Exception("Nothing returned when grabbing the filename");
                        }
                    }
                }
            }
        }

        [CLSCompliant(false)]
        public override IDTSOutput100 InsertOutput(DTSInsertPlacement insertPlacement, int outputID)
        {
            throw new Exception("You cannot insert an output on this component");
        }

        [CLSCompliant(false)]
        public override DTSValidationStatus Validate()
        {
            bool pbCancel = false;

            if (ComponentMetaData.OutputCollection.Count != 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Unexpected Output found. Destination components do not support outputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.RuntimeConnectionCollection["File To Write"].ConnectionManager == null)
            {
                ComponentMetaData.FireError(0, "Validate", "No Connection Manager returned", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.AreInputColumnsValid == false)
            {
                ComponentMetaData.InputCollection["Component Input"].InputColumnCollection.RemoveAll();
                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
            }

            return base.Validate();
        }

        public override void ReinitializeMetaData()
        {
            IDTSInput100 _profinput = ComponentMetaData.InputCollection["Component Input"];
            _profinput.InputColumnCollection.RemoveAll();

            IDTSVirtualInput100 vInput = _profinput.GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vCol in vInput.VirtualInputColumnCollection)
            {
                this.SetUsageType(_profinput.ID, vInput, vCol.LineageID, DTSUsageType.UT_READONLY);
            }
        }

        public override void OnInputPathAttached(int inputID)
        {
            IDTSInput100 input = ComponentMetaData.InputCollection.GetObjectByID(inputID);
            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            foreach (IDTSVirtualInputColumn100 vCol in vInput.VirtualInputColumnCollection)
            {
                this.SetUsageType(inputID, vInput, vCol.LineageID, DTSUsageType.UT_READONLY);
            }
        }

        public override void DeleteInput(int inputID)
        {
            throw new Exception("You cannot delete an input from this component.");
        }

        [CLSCompliant(false)]
        public override IDTSInput100 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            throw new Exception("You cannot add an Input to this component.");
        }

        #endregion

        #region Runtime

        public override void PreExecute()
        {
            IDTSInput100 input = ComponentMetaData.InputCollection["Component Input"];

            foreach (IDTSInputColumn100 inCol in input.InputColumnCollection)
            {
                ColumnInfo ci = new ColumnInfo();
                ci.BufferColumnIndex = BufferManager.FindColumnByLineageID(input.Buffer, inCol.LineageID);
                ci.ColumnName = inCol.Name;
                _columnInfos.Add(ci);
            }

            // Open the file
            _fs = new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.Write);
            _sw = new StreamWriter(_fs);
        }

        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            while (buffer.NextRow())
            {
                _sw.WriteLine("<START>");
                for (int i = 0; i < _columnInfos.Count; i++)
                {
                    ColumnInfo ci = (ColumnInfo)_columnInfos[i];

                    if (buffer.IsNull(ci.BufferColumnIndex))
                    {
                        _sw.WriteLine(ci.ColumnName + ":");
                    }
                    else
                    {
                        _sw.WriteLine(ci.ColumnName + ":" +
                           buffer[ci.BufferColumnIndex].ToString());
                    }
                }
                _sw.WriteLine("<END>");
            }
            if (buffer.EndOfRowset) _sw.Close();
        }


        [CLSCompliant(false)]
        public override IDTSInputColumn100 SetUsageType(int inputID, IDTSVirtualInput100 virtualInput, int lineageID, DTSUsageType usageType)
        {
            IDTSInputColumn100 inp = base.SetUsageType(inputID, virtualInput, lineageID, usageType);

            if (inp != null)
            {
                if (inp.UsageType == DTSUsageType.UT_READWRITE)
                {
                    throw new Exception("You cannot set a column to read write for this destination");
                }
            }

            return inp;
        }

        #endregion

        #endregion

        #region Helpers
        private string ReturnConnectionObject(IDTSRuntimeConnectionCollection100 cns, string name, object tns)
        {
            return cns[name].ConnectionManager.AcquireConnection(tns).ToString();
        }
        #endregion
    }
}
