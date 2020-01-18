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

namespace MOMDemo
{
    public partial class PackageListings : Form
    {
        public PackageListings()
        {
            InitializeComponent();
        }

        private void PackageListings_Load(object sender, EventArgs e)
        {
            Server server = new Server(@"localhost\rc0");

            try
            {
                IntegrationServices isServer = new IntegrationServices(server);

                Catalog catalog = null;

                foreach (Catalog c in isServer.Catalogs)
                    catalog = c;

                TreeNode catalogNode = new TreeNode("Catalog: " + catalog.Name);
                treeView1.Nodes.Add(catalogNode);

                foreach (CatalogFolder f in catalog.Folders)
                {
                    TreeNode folderNode = new TreeNode("Folder: " + f.Name);
                    catalogNode.Nodes.Add(folderNode);

                    foreach (ProjectInfo p in f.Projects)
                    {
                        TreeNode projectNode = new TreeNode("Project: " + p.Name);
                        folderNode.Nodes.Add(projectNode);

                        foreach (Microsoft.SqlServer.Management.IntegrationServices.PackageInfo pkg in p.Packages)
                        {
                            TreeNode packageNode = new TreeNode("Package: " + pkg.Name);
                            projectNode.Nodes.Add(packageNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
