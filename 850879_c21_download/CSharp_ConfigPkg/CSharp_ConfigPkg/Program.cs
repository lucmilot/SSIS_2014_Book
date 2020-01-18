using System;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace CSharp_ConfigPkg
{
    class Program
    {
        static void Main(string[] args)
        {
      CreatePackageConfig(@"package.dtsx");
        }

        private static void CreatePackageConfig(string PackagePath)
        {
            Application app = new Application();
            Package pkg = app.LoadPackage(PackagePath, null);
            Variable var = pkg.Variables.Add("myConfigVar", false, "", "Test");
            string packagePathToVariable = var.GetPackagePath();

            pkg.EnableConfigurations = true;

            Configuration config = pkg.Configurations.Add();
            config.ConfigurationString = "ConfigureMyConfigVar";
            config.ConfigurationType = DTSConfigurationType.EnvVariable;
            config.Name = "ConfigureMyConfigVar";
            config.PackagePath = packagePathToVariable;
            app.SaveToXml(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\mytestSSISPackageConfig.xml", pkg, null);
            Console.WriteLine("Configuration Created and Saved");
        }
    }
}
