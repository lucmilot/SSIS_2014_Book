using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.IntegrationServices;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Dts.Runtime;

/* if you run into any problems with this code, feel free to email me at chrisrock2@gmail.com */

namespace MOMDemo
{
    public partial class Form1 : Form
    {
        private string _server = @"localhost";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server server = new Server(_server);
           
            try
            {
                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = new Catalog(isServer, "SSISDB", "password");
                catalog.Create();

                MessageBox.Show("Catalog created");
                //connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
              
            }
        }

        private void Create_Click(object sender, EventArgs e)
        {
             Server server = new Server(_server);
          
            try
            {
                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];
                
                CatalogFolder folder = new CatalogFolder(catalog, "ProSSIS", "New Folder");
                folder.Create();

                MessageBox.Show("Folder Created");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateProject_Click(object sender, EventArgs e)
        {
            try
            {
                string projectFileName = @"myproject.ispac";
                using (Project project = Project.CreateProject(projectFileName))
                {
                    project.Name = "My ProSSIS Project";

                    project.Description = "This is a new project";

                    project.PackageItems.Add(new Package(), "package.dtsx");
                    project.PackageItems[0].Package.Description = "Package Description";

                    project.Save();

                    MessageBox.Show("Project created");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CreateProjectWithParamsButton_Click(object sender, EventArgs e)
        {
            try
            {
                string projectFileName = @"myproject.ispac";
                using (Project project = Project.CreateProject(projectFileName))
                {
                    project.Name = "My ProSSIS Project";

                    project.Description = "This is a new project";

                    project.PackageItems.Add(new Package(), "package.dtsx");

                    project.PackageItems[0].Package.Description = "Package Description";

                    Package package = project.PackageItems[0].Package;
                    // add the package parameter
                    package.Parameters.Add("PackageParameter1", TypeCode.String);

                    // add the project parameter
                    project.Parameters.Add("ProjectParameter2", TypeCode.String);

                    // add the package parameter value by accessing the parameter using the parameter name
                    package.Parameters["PackageParameter1"].Value = "Value for the package parameter";

                    // add the project parameter value by accessing the parameter using the index
                    project.Parameters[0].Value = "Value for the project parameter";

                    package.Properties["Description"].SetExpression(package, "@[$Project::ProjectParameter2]");

                    project.Save();

                    MessageBox.Show("Project created");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeployProject_Click(object sender, EventArgs e)
        {

             
             try
             {
                 Server server = new Server(_server);

                 IntegrationServices isServer = new IntegrationServices(server);

                 Catalog catalog = isServer.Catalogs["SSISDB"];

                 CatalogFolder folder = catalog.Folders["ProSSIS"];


                 string projectFileName = "myproject.ispac";

                 folder.DeployProject("My ProSSIS Project", System.IO.File.ReadAllBytes(projectFileName));
                 folder.Alter();

                 ProjectInfo p = folder.Projects["My ProSSIS Project"];

                 p.References.Add("Environment1", folder.Name);
                 p.Alter();

                 MessageBox.Show("Project deployed");

                 //using (Project project = Project.OpenProject(projectFileName))
                 //{
                     

                 //    MessageBox.Show("Project deployed");
                 //}
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
        }

        private void CreateEnvironmentButton_Click(object sender, EventArgs e)
        {
            

            try
            {
                Server server = new Server(_server);

                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];

                CatalogFolder folder = catalog.Folders["ProSSIS"];

                EnvironmentInfo env = new EnvironmentInfo(folder, "Environment1", "Description of Environment1");
                env.Create();

                env.Variables.Add("var1", TypeCode.Int32, 1, false, "Var1 Description");
                env.Variables.Add("sensitiveVar2", TypeCode.String, "secure value", true, "");
                env.Alter();

                //ProjectInfo p = folder.Projects["ProSSIS"];

                //p.References.Add("Environment1", folder.Name);
                //p.Alter();

                MessageBox.Show("Environment Created");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExecutePackageButton_Click(object sender, EventArgs e)
        {
            
                
                try
                {
                    Server server = new Server(_server);


                    IntegrationServices isServer = new IntegrationServices(server);
                    
                    Catalog catalog = isServer.Catalogs["SSISDB"];

                    CatalogFolder folder = catalog.Folders["ProSSIS"];

                    ProjectInfo p = folder.Projects["My ProSSIS Project"];

                    Microsoft.SqlServer.Management.IntegrationServices.PackageInfo pkg = p.Packages["package.dtsx"];
                    EnvironmentReference reference = p.References["Environment1", folder.Name];
                    reference.Refresh();

                    long operationId = pkg.Execute(false, reference);
                    
                    catalog.Operations.Refresh();
                    StringBuilder messages = new StringBuilder();

                    foreach (Operation op in catalog.Operations)
                    {
                        if (op.Id == operationId)
                        {
                            op.Refresh();

                            foreach (OperationMessage msg in op.Messages)
                            {
                                messages.AppendLine(msg.Message);
                            }
                        }
                    }

                    LogFileTextbox.Text = "Package executed: " + messages.ToString();

                    //MessageBox.Show("Package executed");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            
        }

        private void DropFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                Server server = new Server(_server);

                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];

                CatalogFolder folder = new CatalogFolder(catalog, "Test", "Test description");// catalog.Folders["ProSSIS"];
                folder.Create();

                EnvironmentInfo newEnv = new EnvironmentInfo(folder, "Environment1", "Description of Environment1");
                newEnv.Create();

                newEnv.Variables.Add("var1", TypeCode.Int32, 1, false, "Var1 Description");
                newEnv.Variables.Add("sensitiveVar2", TypeCode.String, "secure value", true, "");
                newEnv.Alter();

                // this will fail because there is an environment under the folder
                try
                {
                    folder.Drop();
                }
                catch
                {
                }

                
                foreach (EnvironmentInfo env in folder.Environments.ToArray())
                {
                    env.Drop();
                }

                foreach (ProjectInfo p in folder.Projects.ToArray())
                {
                    p.Drop();
                }

                // this will succeed now that everything has been removed. 
                folder.Drop();

                MessageBox.Show("Folder removed");

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TransferPackageButton_Click(object sender, EventArgs e)
        {
            try
            {
                Server server = new Server(_server);

                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];

                CatalogFolder folder = new CatalogFolder(catalog, "Test", "Test description");// catalog.Folders["ProSSIS"];
                folder.Create();

                Project localProject = CreateNewProject("newproject.ispac", "New Project");

                folder.DeployProject("New Project", System.IO.File.ReadAllBytes("newprojects.ispac"));
                folder.Alter();

                ProjectInfo newProject = folder.Projects["New Project"];
                newProject.Move("ProSSIS");
                newProject.Alter();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Project CreateNewProject(string projectFileName, string projectName)
        {
            Project project = Project.CreateProject(projectFileName);
            project.Name = projectName;

            project.Description = "This is a new project";

            project.PackageItems.Add(new Package(), "package.dtsx");
            project.PackageItems[0].Package.Description = "Package Description";

            Package package = project.PackageItems[0].Package;
            // add the package parameter
            package.Parameters.Add("PackageParameter1", TypeCode.String);

            // add the project parameter
            project.Parameters.Add("ProjectParameter2", TypeCode.String);

            // add the package parameter value by accessing the parameter using the parameter name
            package.Parameters["PackageParameter1"].Value = "Value for the package parameter";

            // add the project parameter value by accessing the parameter using the index
            project.Parameters[0].Value = "Value for the project parameter";

            package.Properties["Description"].SetExpression(package, "@[$Project::ProjectParameter2]");

            project.Save();


            return project;
        }

        private void ShowRunningPackagesButton_Click(object sender, EventArgs e)
        {
            Server server = new Server(_server);

            try
            {
                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];

                StringBuilder messages = new StringBuilder();

                foreach (ExecutionOperation exec in catalog.Executions)
                {
                    catalog.Executions.Refresh();

                    if (!exec.Completed)
                    {
                        messages.AppendLine("Package " + exec.PackageName + " is running. The project is " + exec.ProjectName + ". It started running at " + exec.StartTime.ToString());
                    }
                }

                LogFileTextbox.Text = messages.ToString();

                //MessageBox.Show("Package executed");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void ShowCatalogButton_Click(object sender, EventArgs e)
        {
            PackageListings pl = new PackageListings();
            pl.ShowDialog();
        }

        private void ShowOperationMessagesButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                Server server = new Server(_server);

                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = isServer.Catalogs["SSISDB"];

                CatalogFolder folder = catalog.Folders["ProSSIS"];

                ProjectInfo p = folder.Projects["My ProSSIS Project"];
                                
                catalog.Operations.Refresh();
                StringBuilder messages = new StringBuilder();

                foreach(ExecutionOperation exec in catalog.Executions)
                {
                    if (exec.Completed)
                    {
                        messages.AppendLine(exec.PackageName + " completed " + exec.EndTime.ToString());

                        var ops = from a in catalog.Operations where a.Id == exec.Id select a;
                    
                        foreach(Operation op in ops)
                        {
                            op.Refresh();

                            foreach (OperationMessage msg in op.Messages)
                            {
                                messages.AppendLine("\t" + msg.Message);
                            }
                        }
                    }
                }

                LogFileTextbox.Text = messages.ToString();

                //MessageBox.Show("Package executed");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
