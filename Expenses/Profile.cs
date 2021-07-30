using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ExpenseTracker
{
    public class Profile
    {
        public string DbStorageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ExpenseTracker"
            );
        public string DbName { get; }
        public string DbPath { get; }
        public string Name { get; }

        private SQLiteConnection con;

        private Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

        public Profile(string name)
        {
            this.Name = name;
            this.DbName = $"database_{name}.sqlite";
            this.DbPath = CheckDB();
            Console.WriteLine(this.DbPath);
        }

        private string CheckDB()
        {
            if (!Directory.Exists(DbStorageFolder))
            {
                Directory.CreateDirectory(this.DbStorageFolder);
            }

            string DbPath = Path.Combine(this.DbStorageFolder, this.DbName);

            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
            }
            return DbPath;
        }

        public void AddAccount(string accountName, string bankName)
        {
            Accounts.Add(accountName, new Account(accountName, bankName));
        }

        public Account getAccount(string accountName)
        {
            return Accounts[accountName];
        }
    }
}
