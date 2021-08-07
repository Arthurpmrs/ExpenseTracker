using System.Data.SQLite;
using System.Dynamic;
using System.Collections.Generic;
using System;

namespace ExpenseTracker
{
    class ChannelDBWrapper
    {
        public string ConnectionString { get; }

        private DBHandler DB;

        public ChannelDBWrapper(DBHandler db)
        {
            this.DB = db;
            this.ConnectionString = db.connectionString;
        }

        public long InsertCannel(string type, string channelName, string identifier, long accountID)
        {
            long rowID;
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
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public Channel GetChannelByName(string channelName)
        {
            Channel channel;
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
                         channel = Factory.Select(
                            this.DB,
                            reader["type"].ToString(),
                            reader.GetInt32(4),
                            reader["name"].ToString(),
                            reader["identifier"].ToString(),
                            reader.GetInt32(0)
                            );
                    }
                    else
                    {
                        channel = null;
                    }
                }
            }
            return channel;
        }
        public List<Channel> GetAllChannels(long accountID)
        {
            List<Channel> Channels = new List<Channel>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM channel WHERE account_id = @accountID";
                    cmd.Parameters.AddWithValue("@accountID", accountID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Channel channel = Factory.Select(
                                this.DB,
                                reader["type"].ToString(),
                                reader.GetInt32(4),
                                reader["name"].ToString(),
                                reader["identifier"].ToString(),
                                reader.GetInt32(0)
                            );
                            Channels.Add(channel);
                        }
                    }
                    else
                    {
                        Channels = null;
                    }
                }
            }
            return Channels;
        }
    }
}
