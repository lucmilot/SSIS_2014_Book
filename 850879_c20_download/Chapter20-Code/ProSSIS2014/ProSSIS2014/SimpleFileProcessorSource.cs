#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
#endregion  

//,IconResource = "Wrox.Pipeline.SimpleFileProcessorSource.ico"
namespace Wrox.Pipeline
{
    [DtsPipelineComponent(
DisplayName = "Wrox Simple File Processor Source Adapter",
ComponentType = ComponentType.SourceAdapter)]
    public class SimpleFileProcessorSource : PipelineComponent
    {
        private Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType _fil;
        private string _filename;

        public override void ProvideComponentProperties()
        {
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();
            RemoveAllInputsOutputsAndCustomProperties();
            ComponentMetaData.Name = "Wrox Simple File Processor";
            ComponentMetaData.Description = "Our first Source Adapter";
            ComponentMetaData.ContactInfo = "www.wrox.com";

            IDTSRuntimeConnection100 rtc = ComponentMetaData.RuntimeConnectionCollection.New();
            rtc.Name = "File To Read";
            rtc.Description = "This is the file from which we want to read";
            IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
            output.Name = "Component Output";
            output.Description = "This is what downstream Components will see";
            output.ExternalMetadataColumnCollection.IsUsed = true;
        }

        public override void AcquireConnections(object transaction)
        {
            if (ComponentMetaData.RuntimeConnectionCollection["File To Read"].ConnectionManager != null)
            {
                ConnectionManager cm = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(ComponentMetaData.RuntimeConnectionCollection["File To Read"].ConnectionManager);
                if (cm.CreationName != "FILE")
                {
                    throw new Exception("The Connection Manager is not a FILE Connection Manager");
                }
                else
                {

                    _fil = (Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType)cm.Properties["FileUsageType"].GetValue(cm);
                    if (_fil != Microsoft.SqlServer.Dts.Runtime.DTSFileConnectionUsageType.FileExists)
                    {
                        throw new Exception("The type of FILE connection manager must be an Existing File");
                    }
                    else
                    {
                        _filename = ComponentMetaData.RuntimeConnectionCollection["File To Read"].ConnectionManager.AcquireConnection(transaction).ToString();
                        if (_filename == null || _filename.Length == 0)
                        {
                            throw new Exception("Nothing returned when grabbing the filename");
                        }
                    }
                }
            }

        }

        [CLSCompliant(false)]
        public override DTSValidationStatus Validate()
        {
            bool pbCancel = false;

            IDTSOutput100 output =
            ComponentMetaData.OutputCollection["Component Output"];
            if (ComponentMetaData.InputCollection.Count != 0)
            {

            ComponentMetaData.FireError(0,ComponentMetaData.Name, "Unexpected input found. Source components do not support inputs.", "", 0, out pbCancel);
            return DTSValidationStatus.VS_ISCORRUPT;

            }
            if (ComponentMetaData.RuntimeConnectionCollection["File To Read"].
            ConnectionManager == null)
            {
            ComponentMetaData.FireError(0, "Validate", "No Connection Manager Specified.", "", 0, out pbCancel);
            return DTSValidationStatus.VS_ISBROKEN;
            }
            // Check for Output Columns, if not then forceReinitializeMetaData
            if (ComponentMetaData.OutputCollection["Component Output"].OutputColumnCollection.Count == 0)

            {

            ComponentMetaData.FireError(0, "Validate", "No output columns specified. Making call to ReinitializeMetaData.", "", 0, out pbCancel);
            return DTSValidationStatus.VS_NEEDSNEWMETADATA;

            }
            //What about if we have output columns but we have noExternalMetaData
            // columns? Maybe somebody removed them through code.
            if (DoesEachOutputColumnHaveAMetaDataColumnAndDoDatatypesMatch(output.ID)== false)
            {
            ComponentMetaData.FireError(0, "Validate", "Output columns and metadata columns are out of sync. Making call to ReinitializeMetaData.", "",0, out pbCancel);
            return DTSValidationStatus.VS_NEEDSNEWMETADATA;

            }
            return base.Validate();
        }

        private bool DoesEachOutputColumnHaveAMetaDataColumnAndDoDatatypesMatch(int id)
        {
            //return true;
            IDTSOutput100 output = ComponentMetaData.OutputCollection.GetObjectByID(id);
            IDTSExternalMetadataColumn100 mdc;
            bool rtnVal = true;
            foreach (IDTSOutputColumn100 col in output.OutputColumnCollection)
            {
                if (col.ExternalMetadataColumnID == 0)
                {
                    rtnVal = false;
                }
                else
                {
                    mdc = output.ExternalMetadataColumnCollection.GetObjectByID
                   (col.ExternalMetadataColumnID);
                    if (mdc.DataType != col.DataType || mdc.Length != col.Length ||
                    mdc.Precision != col.Precision || mdc.Scale != col.Scale ||
                    mdc.CodePage != col.CodePage)
                    {
                        rtnVal = false;
                    }
                }
            }
            return rtnVal; 

        }

        public override void ReinitializeMetaData()
        {

            IDTSOutput100 _profoutput = ComponentMetaData.OutputCollection["Component Output"];
            if (_profoutput.ExternalMetadataColumnCollection.Count > 0)
            {
                _profoutput.ExternalMetadataColumnCollection.RemoveAll();
            }
            if (_profoutput.OutputColumnCollection.Count > 0)
            {
                _profoutput.OutputColumnCollection.RemoveAll();
            }

            CreateOutputAndMetaDataColumns(_profoutput);
        }


        private void CreateOutputAndMetaDataColumns(IDTSOutput100 output)
        {

            IDTSOutputColumn100 outName = output.OutputColumnCollection.New();
            outName.Name = "Name";
            outName.Description = "The Name value retrieved from File";
            outName.SetDataTypeProperties(DataType.DT_STR, 50, 0, 0, 1252);

            CreateExternalMetaDataColumn(output.ExternalMetadataColumnCollection, outName);

            IDTSOutputColumn100 outAge = output.OutputColumnCollection.New();
            outAge.Name = "Age";
            outAge.Description = "The Age value retrieved from File";
            outAge.SetDataTypeProperties(DataType.DT_I4, 0, 0, 0, 0);
            //Create an external metadata column to go alongside with it

            CreateExternalMetaDataColumn(output.ExternalMetadataColumnCollection, outAge);

            IDTSOutputColumn100 outMarried = output.OutputColumnCollection.New();
            outMarried.Name = "Married";
            outMarried.Description = "The Married value retrieved from File";
            outMarried.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            //Create an external metadata column to go alongside withit
            CreateExternalMetaDataColumn(output.ExternalMetadataColumnCollection, outMarried);

            IDTSOutputColumn100 outSalary = output.OutputColumnCollection.New();
            outSalary.Name = "Salary";
            outSalary.Description = "The Salary value retrieved from File";
            outSalary.SetDataTypeProperties(DataType.DT_DECIMAL, 0, 0, 10, 0);
            //Create an external metadata column to go alongside with it
            CreateExternalMetaDataColumn(output.ExternalMetadataColumnCollection, outSalary);
        }

        private void ParseTheFileAndAddToBuffer(string filename, PipelineBuffer buffer)
        {
            TextReader tr = File.OpenText(filename);

            IDTSOutput100 output =
            ComponentMetaData.OutputCollection["Component Output"];

            IDTSOutputColumnCollection100 cols =
            output.OutputColumnCollection;
            IDTSOutputColumn100 col;
            string s = tr.ReadLine();
            int i = 0;
            while (s != null)
            {
                if (s.StartsWith("<START>"))
                    buffer.AddRow();
                if (s.StartsWith("Name:"))
                {
                    col = cols["Name"];
                    i = BufferManager.FindColumnByLineageID(output.Buffer,
                    col.LineageID);
                    string value = s.Substring(5);
                    buffer.SetString(i, value);
                }
                if (s.StartsWith("Age:"))
                {
                    col = cols["Age"];
                    i = BufferManager.FindColumnByLineageID(output.Buffer,
                    col.LineageID);
                    Int32 value;
                    if (s.Substring(4).Trim() == "")
                        value = 0;
                    else
                        value = Convert.ToInt32(s.Substring(4).Trim());
                    buffer.SetInt32(i, value);
                }
                if (s.StartsWith("Married:"))
                {
                    col = cols["Married"];
                    bool value;

                    i = BufferManager.FindColumnByLineageID(output.Buffer,
                    col.LineageID);
                    if (s.Substring(8).Trim() == "")
                        value = true;
                    else
                        value = s.Substring(8).Trim() != "1" ? false : true;
                    buffer.SetBoolean(i, value);
                }
                if (s.StartsWith("Salary:"))
                {
                    col = cols["Salary"];
                    Decimal value;
                    i = BufferManager.FindColumnByLineageID(output.Buffer,
                    col.LineageID);
                    if (s.Substring(7).Trim() == "")
                        value = 0M;
                    else
                        value = Convert.ToDecimal(s.Substring(8).Trim());
                    buffer.SetDecimal(i, value);
                }
                s = tr.ReadLine();

            }
            tr.Close();
        }

        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            ParseTheFileAndAddToBuffer(_filename, buffers[0]);

            buffers[0].SetEndOfRowset();
        }

        private void CreateExternalMetaDataColumn(IDTSExternalMetadataColumnCollection100 output, IDTSOutputColumn100 outputColumn)
        {

            // Set the properties of the external metadata column.
            IDTSExternalMetadataColumn100 externalColumn = output.New();
            externalColumn.Name = outputColumn.Name;
            externalColumn.Precision = outputColumn.Precision;
            externalColumn.Length = outputColumn.Length;
            externalColumn.DataType = outputColumn.DataType;
            externalColumn.Scale = outputColumn.Scale;

            // Map the external column to the output column.
            outputColumn.ExternalMetadataColumnID = externalColumn.ID;

        }

    }
}
