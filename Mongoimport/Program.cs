using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mongoimport
{
    class Program
    {
        static void Main(string[] args)
        {
            string exePath = "";

            if (ConfigurationManager.AppSettings["ServiceAdress"] != null && ConfigurationManager.AppSettings["ServiceAdress"] != "")
            {
                if (File.Exists(ConfigurationManager.AppSettings["ServiceAdress"]))
                {
                    exePath = ConfigurationManager.AppSettings["ServiceAdress"];
                    Console.WriteLine("Use config Mongoimport.exe");
                }
            }

            if (exePath == "" && !isGetEXE("mongoimport.exe", out exePath))
            {
                exePath = System.IO.Directory.GetCurrentDirectory() + "\\Mongoimport.exe";
                Console.WriteLine("Use lcoal Mongoimport.exe");
            }
            else
            {
                Console.WriteLine("Use find Mongoimport.exe");
            }

            



            if (ConfigurationManager.AppSettings["ServiceAdress"] == null || exePath == "" || ConfigurationManager.AppSettings["MongoDBFile"] == null)
            {
                Console.WriteLine("Config file is wrong");
            }
            else
            {
                ImportTool import = new ImportTool();
                if (import.Initial(exePath, ConfigurationManager.AppSettings["MongoDBFile"], ConfigurationManager.AppSettings["ServiceAdress"]))
                {
                    Console.WriteLine("Initialize sucessful \r\n");

                    if (import.ImportDB())
                    {
                        Console.WriteLine("Import completed \r\n");
                    }
                    else
                    {
                        Console.WriteLine("Import failed \r\n");
                    }
                }
                else
                {
                    Console.WriteLine("Initialize failed \r\n");
                }
            }
            //if (ConfigurationManager.AppSettings["ServiceAdress"] == null || ConfigurationManager.AppSettings["mongoimport"] == null || ConfigurationManager.AppSettings["MongoDBFile"] == null)
            //{
            //    Console.WriteLine("Config file is wrong");
            //}
            //else
            //{
            //    ImportTool import = new ImportTool();
            //    if (import.Initial(ConfigurationManager.AppSettings["mongoimport"], ConfigurationManager.AppSettings["MongoDBFile"], ConfigurationManager.AppSettings["ServiceAdress"]))
            //    {
            //        Console.WriteLine("Initialize sucessful \r\n");

            //        if (import.ImportDB())
            //        {
            //            Console.WriteLine("Import completed \r\n");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Import failed \r\n");
            //        }
            //    }
            //    else 
            //    {
            //        Console.WriteLine("Initialize failed \r\n");
            //    }
                               
            //}

            Console.ReadKey();
        }

        public static string GetEXEDlg()
        {

            System.Windows.Forms.OpenFileDialog odlg = new OpenFileDialog();
            odlg.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            odlg.Filter = "exe files (*.exe)|*.exe";
            //odlg.FileName = name;
            if (odlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(odlg.FileName) != "mongoexport")
                {
                    Console.WriteLine("Can not find 'mongoimport.exe'");
                    return null;
                }
                return odlg.FileName;

            }
            else { return null; }
    

        }

        public static bool isGetEXE(string exeName, out string path)
        {
            string local = ProgramFilesX86;
            string exePath;
            List<string> findpath;
            if (Is64bitOS)
            {
                string[] names = local.Split('(');
                 findpath = GetFilesPath(names[0] + @"\MongoDB");
                exePath = HaveFile(findpath, exeName);
                if (exePath != null)
                {
                    path = exePath;
                    return true;
                }
            }

            findpath = GetFilesPath(local + @"\MongoDB");
            exePath = HaveFile(findpath, exeName);
            if (exePath != null)
            {
                path = exePath;
                return true;
            }
            path = null;
            return false;
            
        }

        public static string HaveFile(List<string> files, string name)
        {
            foreach (string path in files)
            {
                if(Path.GetFileName(path) == name)
                {
                    return path;
                }
            }

            return null;
        }
        public static bool Is64bitOS
        {
            get { return (Environment.GetEnvironmentVariable("ProgramFiles(x86)") != null); }
        }


        public static string ProgramFilesX86
        {
            get
            {
                string programFiles = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                if (programFiles == null)
                {
                    programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
                }

                return programFiles;
            }
        }
        static List<string> list = new List<string>();
        public static List<string> GetFilesPath(string path)
        {
            
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                list.Add(f.FullName);//添加文件的路径到列表
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFilesPath(d.FullName);
                //list.Add(d.FullName);//添加文件夹的路径到列表
            }
            return list;
        }
    }  
}
