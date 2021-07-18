using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    class Transaction
    {
        public string Note { get; }
        public string Channel { get; }
        public double Value { get; }
        public DateTime Date { get; }

        public Transaction(double value, string channel, string note, string date = "")
        {
            this.Note = note;
            this.Channel = channel;
            this.Value = value;
            if (date == "")
            {
                this.Date = DateTime.Now;
            } else
            {
                this.Date = DateTime.Parse(date);
            }
        }
    }
}
