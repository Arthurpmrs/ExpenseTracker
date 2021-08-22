using System.Collections.Generic;
using System.Data.SQLite;
using System.Dynamic;
using System;

namespace ExpenseTracker
{
    public class AccountDBWrapper
    {
        public string ConnectionString { get; }

        private DBHandler DB;

        public AccountDBWrapper(DBHandler db)
        {
            this.DB = db;
            this.ConnectionString = db.connectionString;
        }
        public long InsertAccount(string accountName, string bankName)
        {
            long rowID;
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
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public Account GetAccountByName(string accountName)
        {
            Account account;
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
                        account = new Account(
                            this.DB,
                            reader["name"].ToString(),
                            reader["bank"].ToString(),
                            reader.GetInt32(0)
                            );
                    }
                    else
                    {
                        account = null;
                    }
                }
            }
            return account;
        }

        public List<Account> GetAllAccounts()
        {
            List<Account> Accounts = new List<Account>();
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
                            Accounts.Add(new Account(this.DB, reader["name"].ToString(), reader["bank"].ToString(), reader.GetInt32(0)));
                        }
                    }
                    else
                    {
                        Accounts = null;
                    }
                }
            }
            return Accounts;
        }

        public List<Tuple<Transaction, Channel>> GetAccountTransactions(long accountID)
        {
            List<Tuple<Transaction, Channel>> Entrys = new List<Tuple<Transaction, Channel>>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT 
                                trans.rowid, trans.value, trans.tag, trans.note, trans.date, trans.date_added, trans.channel_id,
                                channel.rowid, channel.type, channel.name, channel.account_id, channel.identifier
                                FROM trans
                                JOIN channel
                                ON trans.channel_id = channel.rowid
                                AND account_id = @accountID";
                    cmd.Parameters.AddWithValue("@accountID", accountID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction(
                                this.DB,
                                reader.GetDouble(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5),
                                reader.GetInt32(6),
                                reader.GetInt32(0)
                                );
                            Channel channel = Factory.Select(
                                this.DB,
                                reader.GetString(8),
                                reader.GetInt32(10),
                                reader.GetString(9),
                                reader.GetString(11),
                                reader.GetInt32(6)
                                );
                            Entrys.Add(new Tuple<Transaction, Channel>(transaction, channel));
                        }
                    }
                    else
                    {
                        Entrys = null;
                    }
                }
            }
            return Entrys;
        }
    }
}
