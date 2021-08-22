using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Infrastructure
{
    public class AccountDBHandler : DBHandler
    {
        public AccountDBHandler(string profileName) : base(profileName)
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
                    cmd.CommandText = @"INSERT INTO account VALUES(@accountName, @bankName)";
                    _ = cmd.Parameters.AddWithValue("@accountName", fields.AccountName);
                    _ = cmd.Parameters.AddWithValue("@bankName", fields.BankName);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public override Fields GetByName(string name)
        {
            Fields field;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM account WHERE name = @name";
                    _ = cmd.Parameters.AddWithValue("@name", name);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if ((reader != null && reader.HasRows) && reader.Read())
                    {
                        field = new Fields()
                        {
                            AccountID = reader.GetInt32(0),
                            AccountName = reader["name"].ToString(),
                            BankName = reader["bank"].ToString()
                        };
                    }
                    else
                    {
                        field = null;
                    }
                }
            }
            return field;
        }
    }
}
