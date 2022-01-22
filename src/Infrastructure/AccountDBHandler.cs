using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Domain.Entities;
using Domain.Helpers;

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
        public override List<Fields> GetAll(Fields field = null)
        {
            List<Fields> Accounts = new List<Fields>();
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
                            Accounts.Add(
                                new Fields()
                                {
                                    AccountID = reader.GetInt32(0),
                                    AccountName = reader["name"].ToString(),
                                    BankName = reader["bank"].ToString()
                                });
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

        public override void DeleteBy(Fields field)
        {
            List<Tuple<string, string>> props = FieldsHandler.GetSettedProperties(field);
            if (props.Count > 1)
            {
                throw new InvalidOperationException("Field instance sent has more than one property.");
            }

            if (props[0].Item1 == "AccountID")
            {
                Console.WriteLine(props[0]);
                _DeleteByID(long.Parse(props[0].Item2));
            }
        }

        private void _DeleteByID(long id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"DELETE FROM account WHERE rowid = @id";
                    cmd.CommandText = @"DELETE FROM transfer WHERE account_id = @id";
                    cmd.CommandText = @"DELETE FROM trans WHERE account_id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
