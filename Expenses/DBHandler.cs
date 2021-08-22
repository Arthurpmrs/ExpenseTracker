using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ExpenseTracker
{
    public class DBHandler
    {
        private string DBStorageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ExpenseTracker"
            );
        public string DBName { get; }
        public string DBPath { get; }
        public string connectionString { get; }

        public DBHandler(string profileName)
        {
            this.DBName =  $"database_{profileName}.sqlite";
            this.DBPath = this.CheckDBFile();
            this.connectionString = $"URI=file:{this.DBPath}";
            this.CheckTables();
        }

        private string CheckDBFile()
        {
            if (!Directory.Exists(DBStorageFolder))
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
        private void CheckTables()
        {
            Dictionary<string, string> Tables = new Dictionary<string, string>() {
                { "account", @"CREATE TABLE account(name TEXT, bank TEXT)" },
                { "transfer", @"CREATE TABLE transfer(type TEXT, name TEXT, identifier TEXT, account_id INT, FOREIGN KEY(account_id) REFERENCES account(rowid) ON DELETE SET NULL)" },
                { "trans", @"CREATE TABLE trans(value REAL, tag TEXT, note TEXT, date TEXT, date_added TEXT, transfer_id INT, FOREIGN KEY(channel_id) REFERENCES channel(rowid) ON DELETE SET NULL)"}
            };
            using SQLiteConnection conn = new SQLiteConnection(this.connectionString);
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
            using SQLiteConnection conn = new SQLiteConnection(this.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
