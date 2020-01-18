using System;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace CSharp_LoadPackage
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadPackage();
        }

        public static void LoadPackage()
        {
            Application dtsApp = new Application();
            string TestPackageFullPath = 
               "Package.dtsx";
            Package pac =  dtsApp.LoadPackage(TestPackageFullPath, null);
            Console.Write("Loading Package: {0}\n", pac.Name);
            Console.Write("With {0} Tasks\n", pac.Executables.Count);
            DisplayFilePackageVariables(TestPackageFullPath, true);
            Console.Read();
             
        }

        private static void DisplayFilePackageVariables(string FullPath, 
                                             bool ShowOnlyUserVariables)
        {
            Application app = new Application();
            Package package = app.LoadPackage(FullPath, null);
            string sMsg = "Variable:[{0}] Type:{1} Default Value:{2} IsExpression?:{3}\n";

            foreach(Variable var in package.Variables)
            {
                if ((var.Namespace != "System" &&
                    ShowOnlyUserVariables) ||
                    !ShowOnlyUserVariables) 
                {
                    Console.WriteLine(String.Format(sMsg, var.Name, 
                                    var.DataType.ToString(), 
                                    var.Value.ToString(), 
                                    var.EvaluateAsExpression.ToString()));
                }
            }
        }

    }
}
