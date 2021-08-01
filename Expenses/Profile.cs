using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace ExpenseTracker
{
    public class Profile
    {
        public string Name { get; }

        private DBHandler db;

        private Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

        public Profile(string name)
        {
            this.Name = name;
            this.db = new DBHandler(name);
        }

        public void AddAccount(string accountName, string bankName, bool forceDuplicate = false)
        {

            if (GetAccount(accountName) == null || forceDuplicate == true)
            {
                using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
                conn.Open();
                using SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = @"INSERT INTO account VALUES(@accountName, @bankName)";
                _ = cmd.Parameters.AddWithValue("@accountName", accountName);
                _ = cmd.Parameters.AddWithValue("@bankName", bankName);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            } else
            {
                throw new DuplicateNameException("Account already created. Use forceDuplicate to create anyways.");
            }
        }

        public Account GetAccount(string accountName)
        {
            Object[] arguments = new Object[] { };
            using (SQLiteConnection conn = new SQLiteConnection(this.db.connectionString))
            {
                conn.Open();
                using SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = @"SELECT rowid, * FROM account WHERE name = @name";
                _ = cmd.Parameters.AddWithValue("@name", accountName);
                cmd.Prepare();
                using SQLiteDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    //Account acc = new Account(this.db, reader.GetInt32(0), reader["name"].ToString(), reader["bank"].ToString());
                    arguments = new Object[] { reader.GetInt32(0),
                                            reader["name"].ToString(),
                                            reader["bank"].ToString()};
                    Console.WriteLine(arguments);
                    break;
                }
            }
            
            if (arguments.Length != 0)
            {
                return new Account(this.db, (int)arguments[0], (string)arguments[1], (string)arguments[2]);
            } else
            {
                return null;
            }
        }

        public void ShowAccounts()
        {
            using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"SELECT rowid, * FROM account";
            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Acc. ID: {reader.GetInt32(0)} | Acc. Name: {reader["name"]} | Bank: {reader["bank"]}");
            }
        }
    }
}
