using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Domain.Entities;

namespace Infrastructure
{
    public class TransferDBHandler : DBHandler
    {
        public TransferDBHandler(string profileName) : base(profileName)
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
                    cmd.CommandText = @"INSERT INTO transfer VALUES(@transferType, @transferName, @transferIdentifier, @accountID)";
                    _ = cmd.Parameters.AddWithValue("@transferType", fields.TransferType);
                    _ = cmd.Parameters.AddWithValue("@transferName", fields.TransferName);
                    _ = cmd.Parameters.AddWithValue("@transferIdentifier", fields.TransferIdentifier);
                    _ = cmd.Parameters.AddWithValue("@accountID", fields.AccountID); ;
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                rowID = conn.LastInsertRowId;
            }
            return rowID;
        }
        public override Fields GetByName(string name)
        {
            Fields fields;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM transfer WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if ((reader != null && reader.HasRows) && reader.Read())
                    {
                        fields = new Fields()
                        {
                            TransferID = reader.GetInt32(0),
                            TransferType = reader["type"].ToString(),
                            TransferName = reader["name"].ToString(),
                            TransferIdentifier = reader["identifier"].ToString(),
                            AccountID = reader.GetInt32(4)
                        };
                    }
                    else
                    {
                        fields = null;
                    }
                }
            }
            return fields;
        }

        public override List<Fields> GetAll(Fields field = null)
        {
            List<Fields> Transfers = new List<Fields>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM transfer WHERE account_id = @accountID";
                    cmd.Parameters.AddWithValue("@accountID", field.AccountID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transfers.Add(
                                new Fields()
                                {
                                    TransferID = reader.GetInt32(0),
                                    TransferType = reader["type"].ToString(),
                                    TransferName = reader.GetString(2),
                                    TransferIdentifier=reader["identifier"].ToString(),
                                    AccountID=reader.GetInt32(4)
                                });
                        }
                    }
                    else
                    {
                        Transfers = null;
                    }
                }
            }
            return Transfers;
        }

        public override void DeleteBy(Fields field)
        {
            throw new NotImplementedException();
        }
    }
}
