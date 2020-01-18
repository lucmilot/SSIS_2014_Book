using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Management.IntegrationServices;
using Microsoft.SqlServer.Management.Smo;

public partial class _Default : System.Web.UI.Page
{
  Application dtsapp;
  PackageGroupCollection pgc;
  string ssisServer = @"localhost";

  protected void Page_Load(object sender, EventArgs e)
  {
  }

  protected void TreeView1_Load(object sender, EventArgs e)
  {
    //Clear TreeView and Load root node
    //Load the SqlServer SSIS folder structure into tree view and show all 
    //nodes
    TreeView1.Nodes.Clear();
    //TreeView1.Nodes.Add(new TreeNode("localhost", @"\"));
    Server server = new Server(ssisServer);

    IntegrationServices isServer = new IntegrationServices(server);

    Catalog catalog = null;

    foreach (Catalog c in isServer.Catalogs)
        catalog = c;

    LoadTreeView(catalog);
    TreeView1.ExpandAll();
  }

  protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
  {
        System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"\/");
        if (regEx.Matches(TreeView1.SelectedNode.ValuePath).Count == 2)
        {
            PackageGroupCollection pgc = BuildPackageGroupCollection(TreeView1.SelectedNode.ValuePath);
            LoadGridView(pgc);
            Session.Add("pgc", pgc);
        }
 
  }

  protected void GridView1_RowCommand(object sender, 
                                      GridViewCommandEventArgs e)
  {
      if (e.CommandName  == "Execute")
      {
          pgc = (PackageGroupCollection)Session["pgc"];
          PackageGroup pg = pgc.Item(Convert.ToInt32(e.CommandArgument));
          Thread oThread = new System.Threading.Thread(new
                           System.Threading.ThreadStart(pg.ExecPackage));
          oThread.Start();
          LoadGridView(pgc);
      }
  }

  protected void LoadTreeView(Catalog catalog)
  {
      TreeNode catalogNode = new TreeNode(catalog.Name);
       TreeView1.Nodes.Add(catalogNode);

        foreach (CatalogFolder f in catalog.Folders)
        {
            TreeNode folderNode = new TreeNode(f.Name);
            catalogNode.ChildNodes.Add(folderNode);

            foreach (ProjectInfo p in f.Projects)
            {
                TreeNode projectNode = new TreeNode( p.Name);
                folderNode.ChildNodes.Add(projectNode);

            }
        }
          
  }

  protected void LoadGridView(PackageGroupCollection pgc)
  {
    GridView1.DataSource = pgc;
    GridView1.DataBind();
  }

  protected PackageGroupCollection  BuildPackageGroupCollection(string pathToProject)
  {
      string[] path = pathToProject.Split('/');
      string catalog = path[0];
      string folder = path[1];
      string project = path[2];

      Server server = new Server(ssisServer);

      IntegrationServices service = new IntegrationServices(server);

      Catalog catalogObject = service.Catalogs[catalog];

      CatalogFolder folderObject = catalogObject.Folders[folder];

      ProjectInfo projectObject = folderObject.Projects[project];
      PackageGroupCollection collection = new PackageGroupCollection();

      foreach (Microsoft.SqlServer.Management.IntegrationServices.PackageInfo p in projectObject.Packages)
      {
          PackageGroup g = new PackageGroup(p, server.Name, catalog, folder, project);
          collection.Add(g);
      }

      return collection;
  }

  protected void Button1_Click(object sender, EventArgs e)
  {
    LoadGridView((PackageGroupCollection)Session["pgc"]);
  }
}
