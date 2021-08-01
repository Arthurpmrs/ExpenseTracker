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

        public int channel_id { get; }

        private DBHandler db;

        public Channel(DBHandler db, int channel_id, string name)
        {
            this.channel_id = channel_id;
            this.Name = name;
            this.db = db;
        }
        public List<Transaction> Transactions = new List<Transaction>();

        public virtual void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("YYYY-MM-DD");
            if (date != "")
            {
                date = dateAdded;
            }
            using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"INSERT INTO trans VALUES(@value, @tag, @note, @date, @date_added, @channel_id)";
            cmd.Parameters.AddWithValue("@value", value);
            cmd.Parameters.AddWithValue("@tag", tag);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@date_added", dateAdded);
            cmd.Parameters.AddWithValue("@channel_id", this.channel_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        public virtual void Expend(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("yyyy-MM-dd");
            if (date != "")
            {
                date = dateAdded;
            }
            using SQLiteConnection conn = new SQLiteConnection(this.db.connectionString);
            conn.Open();
            using SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"INSERT INTO trans VALUES(@value, @tag, @note, @date, @date_added, @channel_id)";
            cmd.Parameters.AddWithValue("@value", -value);
            cmd.Parameters.AddWithValue("@tag", tag);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@date_added", dateAdded);
            cmd.Parameters.AddWithValue("@channel_id", this.channel_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }

    public static class Factory
    {
        public static Channel Select(DBHandler db, int channel_id, string channelType, string name = "", string identifier = "")
        {
            switch (channelType)
            {
                default:
                    throw (new KeyNotFoundException($"No such channel as {channelType}"));
                case "transference":
                    return new Transference(db, channel_id, name);
                case "debit":
                    return new Debit(db, channel_id, name, identifier);
                case "credit":
                    return new Credit(db, channel_id, name, identifier);
                case "pix":
                    return new Pix(db, channel_id, name);
                case "money":
                    return new Money(db, channel_id);
            }
        }
    }
    public class Transference: Channel
    {
        public Transference(DBHandler db, int channel_id, string name) : base(db, channel_id, name)
        {
        }
    }

    public class Debit: Channel
    {
        public string Identifier { get; }

        public Debit(DBHandler db, int channel_id, string name, string identifier) : base(db, channel_id, name)
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

        public Credit(DBHandler db, int channel_id, string name, string identifier) : base(db, channel_id, name)
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
        public Pix(DBHandler db, int channel_id, string name) : base(db, channel_id, name)
        {
        }
    }
    public class Money: Channel
    {
        public Money(DBHandler db, int channel_id, string name = "Wallet") : base(db, channel_id, name)
        {

        }
    }
}
