using System;

namespace ExpenseTracker
{
    public class Transaction
    {
        public double Value { get; }
        public string Note { get; }
        public string Tag { get; set; }
        public long ChannelID { get; set; }
        public string Date { get; }
        public string DateAdded { get; }
        public long TransactionID { get; set; }

        private DBHandler DB;

        public Transaction(DBHandler db, double value, string tag , string note , string date , string dateAdded, long channelID, long transactionID = -1)
        {
            this.DB = db;
            this.Value = value;
            this.Tag = tag;
            this.Note = note;
            this.ChannelID = channelID;
            this.TransactionID = transactionID;
            if (date == "")
            {
                date = DateTime.Now.ToString();
                dateAdded = date;
            }
            this.Date = date;
            this.DateAdded = dateAdded;
        }

        public void Save()
        {
            TransactionDBWrapper wrapper = new TransactionDBWrapper(this.DB);
            this.TransactionID = wrapper.InsertTransaction(this.Value, this.Tag, this.Note, this.Date, this.DateAdded, this.ChannelID);
        }

        public void Print()
        {
            Console.WriteLine($"Transaction({this.Date}): R${this.Value}, [{this.Tag}] ... N:{this.Note}");
        }
    }
}
