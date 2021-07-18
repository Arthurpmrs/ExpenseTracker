using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Transaction
    {
        public double Value { get; }
        public string Note { get; }
        public string Tag { get; set; }
        public DateTime Date { get; }

        public Transaction(double value, string tag = "", string note = "", string date = "")
        {
            this.Value = value;
            this.Tag = tag;
            this.Note = note;
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
