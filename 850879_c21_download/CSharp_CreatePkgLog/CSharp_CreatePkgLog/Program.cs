using System;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace CSharp_CreatePkgLog
{
    class Program
    {
        static void Main(string[] args)
        {
             CreatePackageLogProvider("package.dtsx");
        }

        public static void CreatePackageLogProvider(string PackagePath)
        {
            Application dtsapp = new Application();
            Package p = dtsapp.LoadPackage(PackagePath, null);

            ConnectionManager myConnMgr = p.Connections.Add("FILE");
            myConnMgr.Name = "mytest.xml";
            myConnMgr.ConnectionString = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\mytest.xml";

            LogProvider logProvider = p.LogProviders.Add("DTS.LogProviderXMLFile.3");
            logProvider.ConfigString = "mytest.xml";
            p.LoggingOptions.SelectedLogProviders.Add(logProvider);
            p.LoggingOptions.EventFilterKind = DTSEventFilterKind.Inclusion;
            p.LoggingOptions.EventFilter = new string[] { "OnError", "OnWarning", "OnInformation" };
            p.LoggingMode = DTSLoggingMode.Enabled;
            logProvider.OpenLog();
            p.Execute();
        }
    }
}
