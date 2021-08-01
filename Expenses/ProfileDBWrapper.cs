using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Dynamic;

namespace ExpenseTracker
{
    class ProfileDBWrapper
    {
        public string ConnectionString { get; }

        public ProfileDBWrapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void InsertAccount(string accountName, string bankName)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO account VALUES(@accountName, @bankName)";
                    _ = cmd.Parameters.AddWithValue("@accountName", accountName);
                    _ = cmd.Parameters.AddWithValue("@bankName", bankName);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }

            }
        }
        public dynamic GetAccountByName(string accountName)
        {
            dynamic accountFields = new ExpandoObject();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM account WHERE name = @name";
                    _ = cmd.Parameters.AddWithValue("@name", accountName);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if ((reader != null && reader.HasRows) && reader.Read())
                    {
                        accountFields.RowID = reader.GetInt32(0);
                        accountFields.Name = reader["name"].ToString();
                        accountFields.Bank = reader["bank"].ToString();
                    } else
                    {
                        accountFields = null;
                    }
                }
            }
            return accountFields;
        }
        public List<dynamic> GetAllAccounts()
        {
            List<dynamic> Accounts = new List<dynamic>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM account";
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Accounts.Add(new {RowID = reader.GetInt32(0), Name = reader["name"], Bank = reader["bank"]});
                        }
                    } else
                    {
                        Accounts = null;
                    }
                }   
            }
            return Accounts;
            
        }
    }
}
