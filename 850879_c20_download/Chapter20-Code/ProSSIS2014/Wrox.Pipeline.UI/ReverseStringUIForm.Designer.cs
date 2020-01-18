namespace Wrox.Pipeline.UI
{
    partial class ReverseStringUIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbSqlConnections = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.dgColumns = new System.Windows.Forms.DataGridView();
            this.Chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InputCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSqlConnections
            // 
            this.cmbSqlConnections.FormattingEnabled = true;
            this.cmbSqlConnections.Location = new System.Drawing.Point(12, 246);
            this.cmbSqlConnections.Name = "cmbSqlConnections";
            this.cmbSqlConnections.Size = new System.Drawing.Size(188, 21);
            this.cmbSqlConnections.TabIndex = 9;
            this.cmbSqlConnections.SelectedValueChanged += new System.EventHandler(this.cmbSqlConnections_SelectedValueChanged);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(272, 13);
            this.labelDescription.TabIndex = 8;
            this.labelDescription.Text = "Select columns to perform reverse string operation upon.";
            // 
            // dgColumns
            // 
            this.dgColumns.AllowUserToAddRows = false;
            this.dgColumns.AllowUserToDeleteRows = false;
            this.dgColumns.AllowUserToResizeRows = false;
            this.dgColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Chk,
            this.InputCol});
            this.dgColumns.Location = new System.Drawing.Point(13, 37);
            this.dgColumns.MultiSelect = false;
            this.dgColumns.Name = "dgColumns";
            this.dgColumns.RowHeadersVisible = false;
            this.dgColumns.Size = new System.Drawing.Size(381, 186);
            this.dgColumns.TabIndex = 7;
            this.dgColumns.Text = "dgColumns";
            this.dgColumns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgColumns_CellContentClick);
            // 
            // Chk
            // 
            this.Chk.HeaderText = "";
            this.Chk.Name = "Chk";
            this.Chk.Width = 30;
            // 
            // InputCol
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-GB");
            dataGridViewCellStyle2.NullValue = false;
            this.InputCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.InputCol.HeaderText = "Name";
            this.InputCol.Name = "InputCol";
            this.InputCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InputCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InputCol.Width = 230;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(320, 246);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(76, 26);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "Cancel";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(240, 246);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(76, 26);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "&OK";
            // 
            // ReverseStringUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 313);
            this.Controls.Add(this.cmbSqlConnections);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.dgColumns);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Name = "ReverseStringUIForm";
            this.Text = "Reverse String Transformation Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dgColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSqlConnections;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.DataGridView dgColumns;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputCol;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
    }
}