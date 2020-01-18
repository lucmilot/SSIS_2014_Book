using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.SqlServer.Management.IntegrationServices ;
using Microsoft.SqlServer.Management.Smo;

/// <summary>
/// Summary description for PackageGroup
/// </summary>
/// 

public class PackageGroup
{
    public PackageGroup(PackageInfo packageInfo, string server, string catalog, string folder, string project)
    {
        _packageinfo = packageInfo;
        _server = server;
        _folder = folder;
        _catalog = catalog;
        _project = project;
    }

    private PackageInfo _packageinfo;
    private string _server;
    private string _catalog;
    private string _folder;
    private string _project;

    public string PackageName
    {
        get { return _packageinfo.Name; }
    }

  
    public string PackageCatalog
    {
        get { return _catalog; }
    }

    public string PackageFolder
    {
        get { return _folder; }
    }

    public string PackageProject
    {
        get { return _project; }
    }

    public string Status
    {
        get { return GetPackageStatus(); }
    }

    public void ExecPackage()
    {
      Server server = new Server(_server);

      IntegrationServices service = new IntegrationServices(server);

      Catalog catalogObject = service.Catalogs[_catalog];

      CatalogFolder folderObject = catalogObject.Folders[_folder];

      ProjectInfo projectObject = folderObject.Projects[_project];
        
        PackageInfo p = projectObject.Packages[_packageinfo.Name];
        p.Execute(false, null);
        
        //Package p = dtsapp.LoadFromSqlServer(string.Concat(_packageinfo.Folder +
        //            "\\" + _packageinfo.Name), _server, null, null, null);
        //p.Execute();
    }

    private string GetPackageStatus()
    {
        Server server = new Server(_server);

        IntegrationServices service = new IntegrationServices(server);

        Catalog catalog = service.Catalogs[_catalog];

        foreach (ExecutionOperation exec in catalog.Executions)
        {
            if (exec.FolderName == _folder && exec.PackageName == _packageinfo.Name)
            {
                catalog.Executions.Refresh();
                if (!exec.Completed)
                {
                    return "Executing";
                }
            }
        }

        return "Sleeping";

        //RunningPackages rps = dtsapp.GetRunningPackages(_server);
        //foreach (RunningPackage rp in rps)
        //{
        //    if (rp.PackageID == new Guid(_packageinfo.PackageGuid))
        //    {
        //        return "Executing";
        //    }
        //}
        //return "Sleeping";
    }
}

