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
        public abstract List<Fields> GetAll(Fields field = null);
        public abstract void DeleteBy(Fields field);
        public abstract void EditByID(long id, Fields field);
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
