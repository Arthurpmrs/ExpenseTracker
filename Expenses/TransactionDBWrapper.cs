using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ExpenseTracker
{
    class TransactionDBWrapper
    {
        public string ConnectionString { get; }

        private DBHandler DB;

        public TransactionDBWrapper(DBHandler db)
        {
            this.DB = db;
            this.ConnectionString = db.connectionString;
        }

        public long InsertTransaction(double value, string tag, string note, string date, string dateAdded, long channelID)
        {
            long rowID;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO trans VALUES(@value, @tag, @note, @date, @date_added, @channel_id)";
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.Parameters.AddWithValue("@tag", tag);
                    cmd.Parameters.AddWithValue("@note", note);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@date_added", dateAdded);
                    cmd.Parameters.AddWithValue("@channel_id", channelID);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public dynamic GetTransactionByID(int transactionID)
        {
            Transaction transaction;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM trans WHERE rowid = @transaction_id";
                    cmd.Parameters.AddWithValue("@transaction_id", transactionID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if ((reader != null && reader.HasRows) && reader.Read())
                    {
                        transaction = new Transaction(
                            this.DB,
                            reader.GetDouble(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetInt32(6), 
                            reader.GetInt32(0)
                            );
                    }
                    else
                    {
                        transaction = null;
                    }
                }
            }
            return transaction;
        }
    }
}
