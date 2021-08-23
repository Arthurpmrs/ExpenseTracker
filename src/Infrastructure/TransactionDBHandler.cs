using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Infrastructure
{
    public class TransactionDBHandler : DBHandler
    {
        public TransactionDBHandler(string profileName) : base(profileName)
        {
            this.DBName = $"database_{profileName}.sqlite";
            SetupDatabase();
        }
        public override long Insert(Fields fields)
        {
            long rowID;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO trans VALUES(@value, @tag, @note, @date, @date_added, @transfer_id, @account_id)";
                    cmd.Parameters.AddWithValue("@value", fields.TransactionValue);
                    cmd.Parameters.AddWithValue("@tag", fields.TransactionTag);
                    cmd.Parameters.AddWithValue("@note", fields.TransactionNote) ;
                    cmd.Parameters.AddWithValue("@date", fields.TransactionDate);
                    cmd.Parameters.AddWithValue("@date_added", fields.TransactionDateAdded);
                    cmd.Parameters.AddWithValue("@transfer_id", fields.TransferID);
                    cmd.Parameters.AddWithValue("@account_id", fields.AccountID);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public override Fields GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public override List<Fields> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
