using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using Domain.Entities;
using Domain.Helpers;

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
                    cmd.Parameters.AddWithValue("@note", fields.TransactionNote);
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

        public override List<Fields> GetAll(Fields field = null)
        {
            List<Tuple<string, string>> props = FieldsHandler.GetSettedProperties(field);

            if (props[0].Item1 == "AccountID")
            {
                return _GetAllAccountTransactions(long.Parse(props[0].Item2));
            }
            else
            {
                throw new NotImplementedException("Other cases are yet to be implemented");
            }
        }
        private List<Fields> _GetAllAccountTransactions(long accountID)
        {
            List<Fields> Transactions = new List<Fields>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT rowid, * FROM trans WHERE account_id = @accountID";
                    cmd.Parameters.AddWithValue("@accountID", accountID);
                    cmd.Prepare();
                    using SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transactions.Add(
                                new Fields()
                                {
                                    TransactionID = reader.GetInt32(0),
                                    TransactionValue = reader.GetDouble(1),
                                    TransactionTag = reader.GetString(2),
                                    TransactionNote = reader.GetString(3),
                                    TransactionDate = reader.GetString(4),
                                    TransactionDateAdded = reader.GetString(5),
                                    TransferID = reader.GetInt32(6),
                                    AccountID = reader.GetInt32(7)
                                });
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
        public override void DeleteBy(Fields field)
        {
            List<Tuple<string, string>> props = FieldsHandler.GetSettedProperties(field);
            if (props.Count > 1)
            {
                throw new InvalidOperationException("Field instance sent has more than one property.");
            }

            if (props[0].Item1 == "TransactionID")
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
                    cmd.CommandText = @"DELETE FROM trans WHERE rowid = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
