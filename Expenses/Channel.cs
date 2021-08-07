using System;
using System.Collections.Generic;

namespace ExpenseTracker
{
    public abstract class Channel
    {
        public string Name { get; }
        public long AccountID { get; }
        public long ChannelID { get; set; }
        public string Type { get; }
        public string Identifier { get; set; }

        private DBHandler DB;
        
        public Channel(DBHandler db, string type, string name, long accountID, long channelID = -1)
        {
            this.DB = db;
            this.ChannelID = channelID;
            this.Name = name;
            this.AccountID = accountID;
            this.Type = type;
            this.Identifier = "";
        }

        public void Save()
        {
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB);
            this.ChannelID = wrapper.InsertCannel(this.Type, this.Name, this.Identifier, this.AccountID);
        }
        
        public virtual Transaction Deposit(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("YYYY-MM-DD");
            if (date != "")
            {
                date = dateAdded;
            }
            Transaction transaction = new Transaction(this.DB, value, tag, note, date, dateAdded, this.ChannelID);
            transaction.Save();
            return transaction;
        }
        public virtual Transaction Expend(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("yyyy-MM-dd");
            if (date != "")
            {
                date = dateAdded;
            }
            Transaction transaction = new Transaction(this.DB, -value, tag, note, date, dateAdded, this.ChannelID);
            transaction.Save();
            return transaction;
        }
        public virtual Transaction GetTransaction(int transactionID)
        {
            TransactionDBWrapper wrapper = new TransactionDBWrapper(this.DB);
            Transaction transaction = wrapper.GetTransactionByID(transactionID);
            if (transaction != null)
            {
                return transaction;
            }
            throw new InvalidOperationException("No such Transaction on database.");
        }
    }

    public static class Factory
    {
        public static Channel Select(DBHandler db, string type, long accountID, string name = "", string identifier = "", long channelID = -1)
        {
            switch (type)
            {
                default:
                    throw (new KeyNotFoundException($"No such channel as {type}"));
                case "transference":
                    return new Transference(db, type, name, accountID, channelID);
                case "debit":
                    return new Debit(db, type, name, identifier, accountID, channelID);
                case "credit":
                    return new Credit(db, type, name, identifier, accountID, channelID);
                case "pix":
                    return new Pix(db, type, name, accountID, channelID);
                case "money":
                    return new Money(db, type, accountID, channelID);
            }
        }
    }
    public class Transference: Channel
    {
        public Transference(DBHandler db, string type, string name, long accountID, long channelID) : base(db, type, name,  accountID, channelID)
        {
        }
    }

    public class Debit: Channel
    {
        public Debit(DBHandler db, string type, string name, string identifier, long accountID, long channelID) : base(db, type, name, accountID, channelID)
        {
            this.Identifier = identifier;
        }
        public override Transaction Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Credit: Channel
    {
        public Credit(DBHandler db, string type, string name, string identifier, long accountID, long channelID) : base(db, type, name, accountID, channelID)
        {
            this.Identifier = identifier;
        }
        public override Transaction Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Pix: Channel
    {
        public Pix(DBHandler db, string type, string name, long accountID, long channelID) : base(db, type, name, accountID, channelID)
        {
        }
    }
    public class Money: Channel
    {
        public Money(DBHandler db, string type, long accountID, long channelID, string name = "Wallet") : base(db, type, name, accountID, channelID)
        {

        }
    }
}
