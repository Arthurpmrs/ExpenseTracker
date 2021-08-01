using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ExpenseTracker
{
    public abstract class Channel
    {
        public string Name { get; }

        public int AccountID { get; }

        public int ChannelID { get; }

        private DBHandler DB;

        public Channel(DBHandler db, int channel_id, string name, int accountID)
        {
            this.DB = db;
            this.ChannelID = channel_id;
            this.Name = name;
            this.AccountID = accountID;
        }
        public List<Transaction> Transactions = new List<Transaction>();

        public virtual void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("YYYY-MM-DD");
            if (date != "")
            {
                date = dateAdded;
            }
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB.connectionString);
            wrapper.InsertTransaction(value, tag, note, date, dateAdded, this.ChannelID);
        }
        public virtual void Expend(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("yyyy-MM-dd");
            if (date != "")
            {
                date = dateAdded;
            }
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB.connectionString);
            wrapper.InsertTransaction(-value, tag, note, date, dateAdded, this.ChannelID);
        }
        public virtual Transaction GetTransaction(int transactionID)
        {
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB.connectionString);
            dynamic transactionFields = wrapper.GetTransactionByID(transactionID);
            if (transactionFields != null)
            {
                return new Transaction(
                    transactionFields.Value,
                    transactionFields.Tag,
                    transactionFields.Note,
                    transactionFields.Date,
                    transactionFields.DateAdded
                    );
            }
            return transactionFields;
        }
    }

    public static class Factory
    {
        public static Channel Select(DBHandler db, int channel_id, string channelType, int accountID, string name = "", string identifier = "")
        {
            switch (channelType)
            {
                default:
                    throw (new KeyNotFoundException($"No such channel as {channelType}"));
                case "transference":
                    return new Transference(db, channel_id, name, accountID);
                case "debit":
                    return new Debit(db, channel_id, name, identifier, accountID);
                case "credit":
                    return new Credit(db, channel_id, name, identifier, accountID);
                case "pix":
                    return new Pix(db, channel_id, name, accountID);
                case "money":
                    return new Money(db, channel_id, accountID);
            }
        }
    }
    public class Transference: Channel
    {
        public Transference(DBHandler db, int channel_id, string name, int accountID) : base(db, channel_id, name, accountID)
        {
        }
    }

    public class Debit: Channel
    {
        public string Identifier { get; }

        public Debit(DBHandler db, int channel_id, string name, string identifier,int accountID) : base(db, channel_id, name, accountID)
        {
            this.Identifier = identifier;
        }
        public override void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Credit: Channel
    {
        public string Identifier { get; }

        public Credit(DBHandler db, int channel_id, string name, string identifier, int accountID) : base(db, channel_id, name, accountID)
        {
            this.Identifier = identifier;
        }
        public override void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Pix: Channel
    {
        public Pix(DBHandler db, int channel_id, string name, int accountID) : base(db, channel_id, name, accountID)
        {
        }
    }
    public class Money: Channel
    {
        public Money(DBHandler db, int channel_id, int accountID, string name = "Wallet") : base(db, channel_id, name, accountID)
        {

        }
    }
}
