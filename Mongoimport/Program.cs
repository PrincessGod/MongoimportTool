using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongoimport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings["ServiceAdress"] == null || ConfigurationManager.AppSettings["mongoimport"] == null || ConfigurationManager.AppSettings["MongoDBFile"] == null)
            {
                Console.WriteLine("Config file is wrong");
            }
            else
            {
                ImportTool import = new ImportTool();
                import.Initial(ConfigurationManager.AppSettings["mongoimport"], ConfigurationManager.AppSettings["MongoDBFile"], ConfigurationManager.AppSettings["ServiceAdress"]);

                if (import.ImportDB())
                {
                    Console.WriteLine("Import completed");
                }
                else
                {
                    Console.WriteLine("Import failed");
                }
                
            }

            Console.ReadKey();
        }
    }
}
