using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Infrastructure
{
    public class SQLiteHandler
    {
        public string ConnectionString { get; set; }
        public string DBStorageFolder { get; }
        public string DBName { get; }
        public SQLiteHandler(string dbstorageFolder, string dbname)
        {
            this.DBStorageFolder = dbstorageFolder;
            this.DBName = dbname;
        }
        public string GetConnectionString()
        {
            string DBPath = CheckDBFile();
            this.ConnectionString = $"URI=file:{DBPath}";
            CheckTables();
            return this.ConnectionString;
        }
        public string CheckDBFile()
        {
            if (!Directory.Exists(this.DBStorageFolder))
            {
                Directory.CreateDirectory(this.DBStorageFolder);
            }

            string DBPath = Path.Combine(this.DBStorageFolder, this.DBName);

            if (!File.Exists(DBPath))
            {
                SQLiteConnection.CreateFile(DBPath);
            }
            return DBPath;
            
        }
        public void CheckTables()
        {
            Dictionary<string, string> Tables = new Dictionary<string, string>() {
                { "account", @"CREATE TABLE account(name TEXT, bank TEXT)" },
                { "transfer", @"CREATE TABLE transfer(type TEXT, name TEXT, identifier TEXT, account_id INT, FOREIGN KEY(account_id) REFERENCES account(rowid) ON DELETE SET NULL)" },
                { "trans", @"CREATE TABLE trans(value REAL, tag TEXT, note TEXT, date TEXT, date_added TEXT, transfer_id INT, account_id INT, FOREIGN KEY(transfer_id) REFERENCES transfer(rowid) ON DELETE SET NULL, FOREIGN KEY(account_id) REFERENCES account(rowid) ON DELETE SET NULL)"}
            };
            using SQLiteConnection conn = new SQLiteConnection(this.ConnectionString);
            conn.Open();

            using SQLiteCommand cmd = new SQLiteCommand(conn);
            foreach (KeyValuePair<string, string> item in Tables)
            {
                cmd.CommandText = @"SELECT name FROM sqlite_master WHERE type = 'table' AND name = @tableName";
                cmd.Parameters.AddWithValue("@tableName", item.Key);
                cmd.Prepare();

                SQLiteDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    this.CreateTable(item.Value);
                }
                reader.Close();
                reader.Dispose();
            }
            cmd.Dispose();
        }
        private void CreateTable(string command)
        {
            using SQLiteConnection conn = new SQLiteConnection(this.ConnectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
