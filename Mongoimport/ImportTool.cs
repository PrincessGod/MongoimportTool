using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongoimport
{
    class ImportTool
    {
        private string exePath;
        private string dbPath;
        private MongoDBHelper MongoDBHelp;
        public bool Initial(string exeFile, string dbPath, string serverCon)
        {
            if (File.Exists(exeFile) && Directory.Exists(dbPath))
            {
                this.exePath = exeFile;
                this.dbPath = dbPath;

                this.MongoDBHelp = new MongoDBHelper();
                return this.MongoDBHelp.InitMongoDB(serverCon);
            }
            
            if (!File.Exists(exeFile))
            {
                Console.WriteLine("Can not find 'mongoimport.exe'");
            }

            if (!Directory.Exists(dbPath))
            {
                Console.WriteLine("The database directory is wrong");
            }

            return false;
        }
      
        public bool ImportDB()
        {
            if(this.dbPath == null || this.exePath == null)
            {
                Console.WriteLine("Provide paths first");
                return false;
            }

            DirectoryInfo dif = new DirectoryInfo(this.dbPath);
            DirectoryInfo[] difs = dif.GetDirectories();

            if(difs.Length == 0)
            {
                Console.WriteLine("Database path don't have any database");
                return false;
            }

            foreach (DirectoryInfo info in difs)
            {
                var files = info.GetFiles();
                if (files.Length != 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Extension == ".json")
                        {
                            this.ImportOneCollection(info.Name, Path.GetFileNameWithoutExtension(file.FullName), file.FullName);
                        }
                    }
                }
            }
            return true;
            
        }

        private void ImportOneCollection(string dbName, string collection, string file)
        {
            Process myProcess = new Process();
            string para = string.Format(@"-d {0} -c {1} {2}", dbName, collection, file);
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(this.exePath, para);
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.Start();
            while (!myProcess.HasExited)
            {
                myProcess.WaitForExit();
            }
            Console.WriteLine(string.Format("Import Database: {0,20} Collection: {1,20}", dbName, collection));
        }
    }
}
