using System.Collections.Generic;
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

        //public void InsertAccount(string accountName, string bankName)
        //{
        //    using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
        //    {
        //        conn.Open();
        //        using (SQLiteCommand cmd = new SQLiteCommand(conn))
        //        {
        //            cmd.CommandText = @"INSERT INTO account VALUES(@accountName, @bankName)";
        //            _ = cmd.Parameters.AddWithValue("@accountName", accountName);
        //            _ = cmd.Parameters.AddWithValue("@bankName", bankName);
        //            cmd.Prepare();
        //            cmd.ExecuteNonQuery();
        //        }

        //    }
        //}
        
            
    }
}
