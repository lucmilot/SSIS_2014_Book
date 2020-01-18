namespace MOMDemo
{
    partial class Form1
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
            this.CreateCatalogButton = new System.Windows.Forms.Button();
            this.CreateFolderButton = new System.Windows.Forms.Button();
            this.CreateProject = new System.Windows.Forms.Button();
            this.CreateProjectWithParamsButton = new System.Windows.Forms.Button();
            this.DeployProject = new System.Windows.Forms.Button();
            this.CreateEnvironmentButton = new System.Windows.Forms.Button();
            this.ExecutePackageButton = new System.Windows.Forms.Button();
            this.DropFolderButton = new System.Windows.Forms.Button();
            this.TransferPackageButton = new System.Windows.Forms.Button();
            this.ShowRunningPackagesButton = new System.Windows.Forms.Button();
            this.ShowCatalogButton = new System.Windows.Forms.Button();
            this.ShowOperationMessagesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.LogFileTextbox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateCatalogButton
            // 
            this.CreateCatalogButton.Location = new System.Drawing.Point(59, 19);
            this.CreateCatalogButton.Name = "CreateCatalogButton";
            this.CreateCatalogButton.Size = new System.Drawing.Size(181, 23);
            this.CreateCatalogButton.TabIndex = 0;
            this.CreateCatalogButton.Text = "Create Catalog";
            this.CreateCatalogButton.UseVisualStyleBackColor = true;
            this.CreateCatalogButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // CreateFolderButton
            // 
            this.CreateFolderButton.Location = new System.Drawing.Point(59, 48);
            this.CreateFolderButton.Name = "CreateFolderButton";
            this.CreateFolderButton.Size = new System.Drawing.Size(181, 23);
            this.CreateFolderButton.TabIndex = 1;
            this.CreateFolderButton.Text = "Create ProSSIS Folder";
            this.CreateFolderButton.UseVisualStyleBackColor = true;
            this.CreateFolderButton.Click += new System.EventHandler(this.Create_Click);
            // 
            // CreateProject
            // 
            this.CreateProject.Location = new System.Drawing.Point(59, 107);
            this.CreateProject.Name = "CreateProject";
            this.CreateProject.Size = new System.Drawing.Size(181, 23);
            this.CreateProject.TabIndex = 3;
            this.CreateProject.Text = "Create Project File";
            this.CreateProject.UseVisualStyleBackColor = true;
            this.CreateProject.Click += new System.EventHandler(this.CreateProject_Click);
            // 
            // CreateProjectWithParamsButton
            // 
            this.CreateProjectWithParamsButton.Location = new System.Drawing.Point(286, 107);
            this.CreateProjectWithParamsButton.Name = "CreateProjectWithParamsButton";
            this.CreateProjectWithParamsButton.Size = new System.Drawing.Size(181, 23);
            this.CreateProjectWithParamsButton.TabIndex = 4;
            this.CreateProjectWithParamsButton.Text = "Create Project File With Params";
            this.CreateProjectWithParamsButton.UseVisualStyleBackColor = true;
            this.CreateProjectWithParamsButton.Click += new System.EventHandler(this.CreateProjectWithParamsButton_Click);
            // 
            // DeployProject
            // 
            this.DeployProject.Location = new System.Drawing.Point(59, 136);
            this.DeployProject.Name = "DeployProject";
            this.DeployProject.Size = new System.Drawing.Size(181, 23);
            this.DeployProject.TabIndex = 5;
            this.DeployProject.Text = "Deploy Project";
            this.DeployProject.UseVisualStyleBackColor = true;
            this.DeployProject.Click += new System.EventHandler(this.DeployProject_Click);
            // 
            // CreateEnvironmentButton
            // 
            this.CreateEnvironmentButton.Location = new System.Drawing.Point(59, 77);
            this.CreateEnvironmentButton.Name = "CreateEnvironmentButton";
            this.CreateEnvironmentButton.Size = new System.Drawing.Size(181, 23);
            this.CreateEnvironmentButton.TabIndex = 6;
            this.CreateEnvironmentButton.Text = "Create Environment";
            this.CreateEnvironmentButton.UseVisualStyleBackColor = true;
            this.CreateEnvironmentButton.Click += new System.EventHandler(this.CreateEnvironmentButton_Click);
            // 
            // ExecutePackageButton
            // 
            this.ExecutePackageButton.Location = new System.Drawing.Point(59, 165);
            this.ExecutePackageButton.Name = "ExecutePackageButton";
            this.ExecutePackageButton.Size = new System.Drawing.Size(181, 23);
            this.ExecutePackageButton.TabIndex = 7;
            this.ExecutePackageButton.Text = "Execute Package";
            this.ExecutePackageButton.UseVisualStyleBackColor = true;
            this.ExecutePackageButton.Click += new System.EventHandler(this.ExecutePackageButton_Click);
            // 
            // DropFolderButton
            // 
            this.DropFolderButton.Location = new System.Drawing.Point(6, 19);
            this.DropFolderButton.Name = "DropFolderButton";
            this.DropFolderButton.Size = new System.Drawing.Size(181, 23);
            this.DropFolderButton.TabIndex = 8;
            this.DropFolderButton.Text = "Drop Folder";
            this.DropFolderButton.UseVisualStyleBackColor = true;
            this.DropFolderButton.Click += new System.EventHandler(this.DropFolderButton_Click);
            // 
            // TransferPackageButton
            // 
            this.TransferPackageButton.Location = new System.Drawing.Point(6, 48);
            this.TransferPackageButton.Name = "TransferPackageButton";
            this.TransferPackageButton.Size = new System.Drawing.Size(181, 23);
            this.TransferPackageButton.TabIndex = 9;
            this.TransferPackageButton.Text = "Transfer Project";
            this.TransferPackageButton.UseVisualStyleBackColor = true;
            this.TransferPackageButton.Click += new System.EventHandler(this.TransferPackageButton_Click);
            // 
            // ShowRunningPackagesButton
            // 
            this.ShowRunningPackagesButton.Location = new System.Drawing.Point(6, 77);
            this.ShowRunningPackagesButton.Name = "ShowRunningPackagesButton";
            this.ShowRunningPackagesButton.Size = new System.Drawing.Size(181, 23);
            this.ShowRunningPackagesButton.TabIndex = 10;
            this.ShowRunningPackagesButton.Text = "Show Running Packages";
            this.ShowRunningPackagesButton.UseVisualStyleBackColor = true;
            this.ShowRunningPackagesButton.Click += new System.EventHandler(this.ShowRunningPackagesButton_Click);
            // 
            // ShowCatalogButton
            // 
            this.ShowCatalogButton.Location = new System.Drawing.Point(6, 19);
            this.ShowCatalogButton.Name = "ShowCatalogButton";
            this.ShowCatalogButton.Size = new System.Drawing.Size(181, 23);
            this.ShowCatalogButton.TabIndex = 11;
            this.ShowCatalogButton.Text = "Show Catalog";
            this.ShowCatalogButton.UseVisualStyleBackColor = true;
            this.ShowCatalogButton.Click += new System.EventHandler(this.ShowCatalogButton_Click);
            // 
            // ShowOperationMessagesButton
            // 
            this.ShowOperationMessagesButton.Location = new System.Drawing.Point(6, 48);
            this.ShowOperationMessagesButton.Name = "ShowOperationMessagesButton";
            this.ShowOperationMessagesButton.Size = new System.Drawing.Size(181, 23);
            this.ShowOperationMessagesButton.TabIndex = 12;
            this.ShowOperationMessagesButton.Text = "Show Operation Messages";
            this.ShowOperationMessagesButton.UseVisualStyleBackColor = true;
            this.ShowOperationMessagesButton.Click += new System.EventHandler(this.ShowOperationMessagesButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Step 1 - ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Step 2 - ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Step 3 - ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Step 4 - ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(245, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "- OR -";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Step 5 - ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Step 6 - ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.LogFileTextbox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.CreateCatalogButton);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.CreateFolderButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CreateProject);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CreateProjectWithParamsButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DeployProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CreateEnvironmentButton);
            this.groupBox1.Controls.Add(this.ExecutePackageButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 313);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Operations";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Log";
            // 
            // LogFileTextbox
            // 
            this.LogFileTextbox.Location = new System.Drawing.Point(59, 194);
            this.LogFileTextbox.Multiline = true;
            this.LogFileTextbox.Name = "LogFileTextbox";
            this.LogFileTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogFileTextbox.Size = new System.Drawing.Size(408, 105);
            this.LogFileTextbox.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(245, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(229, 27);
            this.label8.TabIndex = 20;
            this.label8.Text = "Dont forget to change the _server variable in the code to your SQL 2012 instance";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ShowCatalogButton);
            this.groupBox2.Controls.Add(this.ShowRunningPackagesButton);
            this.groupBox2.Controls.Add(this.ShowOperationMessagesButton);
            this.groupBox2.Location = new System.Drawing.Point(21, 331);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 110);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other Operations";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DropFolderButton);
            this.groupBox3.Controls.Add(this.TransferPackageButton);
            this.groupBox3.Location = new System.Drawing.Point(287, 331);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 110);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dont Run - Just look at the code";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 488);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "MOM Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CreateCatalogButton;
        private System.Windows.Forms.Button CreateFolderButton;
        private System.Windows.Forms.Button CreateProject;
        private System.Windows.Forms.Button CreateProjectWithParamsButton;
        private System.Windows.Forms.Button DeployProject;
        private System.Windows.Forms.Button CreateEnvironmentButton;
        private System.Windows.Forms.Button ExecutePackageButton;
        private System.Windows.Forms.Button DropFolderButton;
        private System.Windows.Forms.Button TransferPackageButton;
        private System.Windows.Forms.Button ShowRunningPackagesButton;
        private System.Windows.Forms.Button ShowCatalogButton;
        private System.Windows.Forms.Button ShowOperationMessagesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox LogFileTextbox;
    }
}

