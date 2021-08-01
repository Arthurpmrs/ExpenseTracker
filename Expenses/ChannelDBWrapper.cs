using System.Data.SQLite;
using System.Dynamic;

namespace ExpenseTracker
{
    class ChannelDBWrapper
    {
        public string ConnectionString { get; }
        public ChannelDBWrapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public void InsertTransaction(double value, string tag, string note, string date, string dateAdded, int channelID)
        {
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
            }
        }
        public dynamic GetTransactionByID(int transactionID)
        {
            dynamic transactionFields = new ExpandoObject();
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
                        transactionFields.RowID = reader.GetInt32(0);
                        transactionFields.Value = reader.GetDouble(1);
                        transactionFields.Tag = reader.GetString(2);
                        transactionFields.Note = reader.GetString(3);
                        transactionFields.Date = reader.GetString(4);
                        transactionFields.DateAdded = reader.GetString(5);
                        transactionFields.ChannelID = reader.GetInt32(6);
                    }
                    else
                    {
                        transactionFields = null;
                    }
                }
            }
            return transactionFields;
        }
    }
}
