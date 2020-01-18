using System;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace CSharp_GetPkgLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPackageLogsForPackage("package.dtsx");
        }
        private static void GetPackageLogsForPackage(string PackagePath)
        {
            Application dtsapp = new Application();
            Package p = dtsapp.LoadPackage(PackagePath, null);
            Console.WriteLine("Executing Package {0}", PackagePath);
            p.Execute();

            Console.WriteLine("Package Execution Complete");
            Console.WriteLine("LogProviders");
            LogProviders logProviders = p.LogProviders;
            Console.WriteLine("LogProviders Count: {0}", logProviders.Count);
            LogProviderEnumerator logProvidersEnum = logProviders.GetEnumerator();

            while (logProvidersEnum.MoveNext())
            {
                LogProvider logProv = logProvidersEnum.Current;
                Console.WriteLine("ConfigString:   {0}", logProv.ConfigString);
                Console.WriteLine("CreationName    {0}", logProv.CreationName);
                Console.WriteLine("DelayValidation {0}", logProv.DelayValidation);
                Console.WriteLine("Description     {0}", logProv.Description);
                Console.WriteLine("HostType        {0}", logProv.HostType);
                Console.WriteLine("ID              {0}", logProv.ID);
                Console.WriteLine("InnerObject     {0}", logProv.InnerObject);
                Console.WriteLine("Name            {0}", logProv.Name);
                Console.WriteLine("-----------------");
            }
            Console.Read();
        }

    }
}
