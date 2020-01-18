using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.SqlServer.Dts.Runtime;
using System.Collections;

namespace Wrox.Pipeline.UI
{
    public partial class ReverseStringUIForm : Form
    {
        #region Local Variables
        private IDTSComponentMetaData100 _dtsComponentMetaData;
        private IDTSDesigntimeComponent100 _designTimeComponent;
        private IDTSInput100 _input;
        private IDTSVirtualInput100 _virtualInput;
        #endregion

        // Constant to define the type of connection we support and wish to work with.
        private Microsoft.SqlServer.Dts.Runtime.Connections _connections;
        private IDtsConnectionService _dtsConnectionService;
        private const string Connection_Type = "ADO.NET:System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

        #region Ctor
        /// <summary>
        /// Form constructor. Initialize variables we need, and setup form controls
        /// </summary>
        [CLSCompliant(false)]
        public ReverseStringUIForm(IDTSComponentMetaData100 dtsComponentMetaData, IServiceProvider serviceProvider, Connections connections)
        {
            InitializeComponent();

            // Store constructor parameters for later
            _dtsComponentMetaData = dtsComponentMetaData;

            // Get design-time interface for changes and validation
            _designTimeComponent = _dtsComponentMetaData.Instantiate();

            // Get Input
            _input = _dtsComponentMetaData.InputCollection[0];

            // Set any form controls that host component properties here
            // None required for ReverseString component

            // Populate DataGridView with columns
            SetInputVirtualInputColumns();

            _connections = connections;
            // Get IDtsConnectionService and store.
            IDtsConnectionService dtsConnectionService = serviceProvider.GetService(typeof(IDtsConnectionService)) as IDtsConnectionService;
            _dtsConnectionService = dtsConnectionService;

            // Get Connections collection, and get name of currently selected connection.
            string connectionName = "";
            if (_dtsComponentMetaData.RuntimeConnectionCollection[0] != null)
            {
                IDTSRuntimeConnection100 runtimeConnection =
                      _dtsComponentMetaData.RuntimeConnectionCollection[0];
                if (runtimeConnection != null
                   && runtimeConnection.ConnectionManagerID.Length > 0
                   && _connections.Contains(runtimeConnection.ConnectionManagerID))
                {
                    connectionName = _connections[runtimeConnection.ConnectionManagerID].Name;
                }
            }

            // Populate connections combo.
            PopulateConnectionsCombo(this.cmbSqlConnections, Connection_Type,
               connectionName);
        }
        #endregion

        private void PopulateConnectionsCombo(ComboBox comboBox, string connectionType, string selectedItem)
        {
            // Prepare combo box by clearing, and adding the new connection item.
            comboBox.Items.Clear();
            comboBox.Items.Add("<New connection...>");

            // Enumerate connections, but for type supported.
            foreach (ConnectionManager connectionManager in _dtsConnectionService.GetConnections())
            {
                comboBox.Items.Add(connectionManager.Name);
            }

            // Set currently selected connection
            comboBox.SelectedItem = selectedItem;
        }

        #region SetInputVirtualInputColumns
        /// <summary>
        /// Helper function to load DataGridView, one row per input column. Column 
        /// UsageType dictates column selection state.
        /// </summary>
        private void SetInputVirtualInputColumns()
        {
            _virtualInput = _input.GetVirtualInput();
            IDTSVirtualInputColumnCollection100 virtualInputColumnCollection = _virtualInput.VirtualInputColumnCollection;
            IDTSInputColumnCollection100 inputColumns = _input.InputColumnCollection;

            int columnCount = virtualInputColumnCollection.Count;
            for (int i = 0; i < columnCount; i++)
            {
                IDTSVirtualInputColumn100 virtualColumn = virtualInputColumnCollection[i];
                int rowIndex;

                if (virtualColumn.UsageType == DTSUsageType.UT_READONLY || virtualColumn.UsageType == DTSUsageType.UT_READWRITE)
                {
                    rowIndex = this.dgColumns.Rows.Add(new object[] { CheckState.Checked, " " + virtualColumn.Name });
                }
                else
                {
                    rowIndex = this.dgColumns.Rows.Add(new object[] { CheckState.Unchecked, " " + virtualColumn.Name });
                }

                this.dgColumns.Rows[rowIndex].Tag = i;

                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)this.dgColumns.Rows[rowIndex].Cells[0];
                cell.ThreeState = false;
            }
        }
        #endregion

        #region dgColumns_CellContentClick
        /// <summary>
        /// Hanlde column selection events from our DataGridView. Columns are selected or deselected 
        /// by setting the UsageType. Ensure changes are made to the through the managed wrapper (design-time interface)
        /// so that changes are persisted or cancelled based on form result.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void dgColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Get current value and flip boolean to get new value
                bool newValue = !Convert.ToBoolean(dgColumns.CurrentCell.Value);

                // Get the virtual column to work with
                IDTSVirtualInputColumn100 virtualColumn = _virtualInput.VirtualInputColumnCollection[e.RowIndex];

                try
                {
                    // Set the column UsageType to indicate the column is selected or not
                    if (newValue)
                        _designTimeComponent.SetUsageType(_input.ID, _virtualInput, virtualColumn.LineageID, DTSUsageType.UT_READWRITE);
                    else
                        _designTimeComponent.SetUsageType(_input.ID, _virtualInput, virtualColumn.LineageID, DTSUsageType.UT_IGNORED);

                }
                catch (Exception ex)
                {
                    // Catch any error from base class SetUsageType here.
                    // Display simple error message from exception
                    MessageBox.Show(ex.Message, "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Rollback UI selection
                    dgColumns.CancelEdit();
                }
            }
        }
        #endregion

        private void cmbSqlConnections_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox comboxBox = (ComboBox)sender;

            // Check for index 0 and <New Item...>
            if (comboxBox.SelectedIndex == 0)
            {
                // Use connection service to create a new connection.
                ArrayList newConns = _dtsConnectionService.CreateConnection(Connection_Type);
                if (newConns.Count > 0)
                {
                    // A new connection has been created, so populate and select
                    ConnectionManager newConn = (ConnectionManager)newConns[0];
                    PopulateConnectionsCombo(comboxBox, Connection_Type, newConn.Name);
                }
                else
                {
                    // Create connection has been cancelled
                    comboxBox.SelectedIndex = -1;
                }
            }

            // An connection has been selected. Verify it exists and update component.
            if (_connections.Contains(comboxBox.Text))
            {
                // Get the selected connection
                ConnectionManager connectionManager = _connections[comboxBox.Text];

                // Save selected connection
                _dtsComponentMetaData.RuntimeConnectionCollection[0].ConnectionManagerID =
                   _connections[comboxBox.Text].ID;
                _dtsComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager =
                   DtsConvert.GetExtendedInterface(_connections[comboxBox.Text]);
            }

        }
    }
}
