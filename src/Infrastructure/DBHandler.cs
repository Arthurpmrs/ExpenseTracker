using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using Domain.Entities;

namespace Infrastructure
{
    public abstract class DBHandler
    {
        private string DBStorageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ExpenseTracker"
            );
        public string DBName { get; set; }
        public string ConnectionString { get; set; }

        public DBHandler(string profileName)
        {
            this.DBName = $"database_{profileName}.sqlite";
        }

        public void SetupDatabase()
        {
            SQLiteHandler sqlhandler = new SQLiteHandler(this.DBStorageFolder, this.DBName);
            this.ConnectionString = sqlhandler.GetConnectionString();
        }
        public abstract long Insert(Fields fields);
        public abstract Fields GetByName(string name);
        public abstract List<Fields> GetAll();
    }
    public class Fields
    {
        public long AccountID;
        public string AccountName;
        public string BankName;
        public long TransferID;
        public string TransferType;
        public string TransferName;
        public string TransferIdentifier;
        public long TransactionID;
        public double TransactionValue;
        public string TransactionTag;
        public string TransactionNote;
        public string TransactionDate;
        public string TransactionDateAdded;
    }
    public enum HandlerType
    {
        Account,
        Transfer,
        Transaction
    }
    public static class DBHandlerFactory
    {
        public static DBHandler Create(HandlerType type, string profileName)
        {
            switch (type)
            {
                default:
                    throw (new KeyNotFoundException($"No such channel as {type}"));
                case HandlerType.Account:
                    return new AccountDBHandler(profileName);
                case HandlerType.Transfer:
                    return new TransferDBHandler(profileName);
                case HandlerType.Transaction:
                    return new TransactionDBHandler(profileName);
            }
        }
    }
}
