using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongoimport
{
    class MongoDBHelper
    {
        private string con = "mongodb://localhost:27017";
        private MongoClient client;
        private MongoDatabase database;
        private MongoServer server;
        private List<string> DBNames;
        public bool InitMongoDB(string connectionString)
        {
            try
            {
                this.con = connectionString;
                client = new MongoClient(con);
                server = client.GetServer();
                server.Connect();
                DBNames = server.GetDatabaseNames().ToList();
                if (server.Instance.State != MongoServerState.Connected)
                {
                    Console.WriteLine("mongodb数据库连接失败");
                    return false;
                }
           
                Console.WriteLine("mongodb数据库连接成功");
                //Console.WriteLine(server.Instance.GetServerDescription().ToString());
                foreach (string db in DBNames)
                {
                    Console.WriteLine("");
                    Console.WriteLine("DB:  " + db);
                    this.database = this.server.GetDatabase(db);
                    List<string> cols = this.database.GetCollectionNames().ToList();
                    foreach (string col in cols)
                    {
                        Console.WriteLine("\t " + col);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("mongodb数据库连接失败");
                return false;
            }


            return true;
        }

        public void UpdateDBNames()
        {
            this.DBNames = server.GetDatabaseNames().ToList();
        }
    }
}
