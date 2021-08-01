using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ExpenseTracker
{
    public class Account
    {
        public string Name { get; }
        public string Bank { get; }
        public int account_id { get; }
        public double Balance
        {
            get
            {
                double balance = 0;

                foreach (var item in Channels)
                {
                    var channel = item.Value.Transactions;
                    foreach (Transaction transaction in channel)
                    {
                        balance += transaction.Value;
                    }
                }
                return balance;
            }
        }
        private DBHandler db;

        private Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();

        public Account(DBHandler db, int account_id, string name, string bank)
        {
            this.Name = name;
            this.Bank = bank;
            this.account_id = account_id;
            this.db = db;
            try
            {
                AddChannel("transference", "Transferência Bancária");
                Console.WriteLine("AQUI CRIOU A TRANSFERENCIA");
            }
            catch (DuplicateNameException)
            {
                //    if (ex is DuplicateNameException || ex is SQLiteException)
                //    {
                //        // write to a log, whatever...
                //        return;
                //    }
                //    throw;
            }

        }

        public void AddChannel(string type, string channelName, string identifier = "", bool forceDuplicate = false)
        {
            if (GetChannel(channelName) == null || forceDuplicate == true)
            {
                using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
                conn.Open();
                using SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = @"INSERT INTO channel VALUES(@channelType, @channelName, @channelIdentifier, @accountID)";
                _ = cmd.Parameters.AddWithValue("@channelType", type.ToLower());
                _ = cmd.Parameters.AddWithValue("@channelName", channelName);
                _ = cmd.Parameters.AddWithValue("@channelIdentifier", identifier);
                _ = cmd.Parameters.AddWithValue("@accountID", this.account_id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            else
            {
                throw new DuplicateNameException("Channel already created. Use forceDuplicate to create anyways.");
            }
        }

        public Channel GetChannel(string channelName)
        {
            using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"SELECT rowid, * FROM channel WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", channelName);
            cmd.Prepare();
            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Channel ch = Factory.Select(
                    this.db,
                    reader.GetInt32(0),
                    reader["type"].ToString(),
                    reader["name"].ToString(),
                    reader["identifier"].ToString()
                    );
                return ch;
            }
            return null;
        }
        public void History()
        {
            using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"SELECT trans.rowid, trans.value, trans.tag, trans.channel_id, channel.rowid, channel.name FROM trans
                                JOIN channel
                                ON trans.channel_id = channel.rowid";
            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader.GetInt32(0)} | Channel: {reader.GetString(5)} | Value: {reader.GetDouble(1)} | Tag: {reader.GetString(2)}");
            }
        }
    }
}
