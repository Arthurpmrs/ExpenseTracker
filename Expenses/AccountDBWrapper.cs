using System.Collections.Generic;
using System.Data.SQLite;
using System.Dynamic;

namespace ExpenseTracker
{
    class AccountDBWrapper
    {
        public string ConnectionString { get; }

        public AccountDBWrapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void InsertCannel(string type, string channelName, string identifier, int accountID)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO channel VALUES(@channelType, @channelName, @channelIdentifier, @accountID)";
                    _ = cmd.Parameters.AddWithValue("@channelType", type.ToLower());
                    _ = cmd.Parameters.AddWithValue("@channelName", channelName);
                    _ = cmd.Parameters.AddWithValue("@channelIdentifier", identifier);
                    _ = cmd.Parameters.AddWithValue("@accountID", accountID);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                } 
            }
        }
        public dynamic GetChannelByName(string channelName)
        {
            dynamic channelFields = new ExpandoObject();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM channel WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", channelName);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if ((reader != null && reader.HasRows) && reader.Read())
                    {
                        channelFields.RowID = reader.GetInt32(0);
                        channelFields.Type = reader["type"].ToString();
                        channelFields.Name = reader["name"].ToString();
                        channelFields.Identifier = reader["identifier"].ToString();
                        channelFields.AccountID = reader.GetInt32(4);
                    } else
                    {
                        channelFields = null;
                    }
                }
            }
            return channelFields;
        }
        public List<dynamic> GetAccountTransactions(int accountID)
        {
            List<dynamic> Transactions = new List<dynamic>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT 
                                trans.rowid, trans.value, trans.tag, trans.note, trans.date, trans.date_added, trans.channel_id,
                                channel.rowid, channel.type, channel.name, channel.account_id
                                FROM trans
                                JOIN channel
                                ON trans.channel_id = channel.rowid
                                AND account_id = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transactions.Add(new { 
                                RowID = reader.GetInt32(0), 
                                Value = reader.GetDouble(1), 
                                Tag = reader.GetString(2),
                                Note = reader.GetString(3),
                                Date = reader.GetString(4),
                                DateAdded = reader.GetString(5),
                                ChannelID = reader.GetInt32(6),
                                ChannelType = reader.GetString(8),
                                ChannelName = reader.GetString(9),
                                AccountID = reader.GetInt32(10)});
                        }
                    }
                    else
                    {
                        Transactions = null;
                    }
                }
            }
            return Transactions;
        }
    }
}
