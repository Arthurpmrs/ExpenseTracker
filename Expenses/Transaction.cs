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
        public DateTime DateAdded { get; }

        public Transaction(double value, string tag , string note , string date , string dateAdded)
        {
            this.Value = value;
            this.Tag = tag;
            this.Note = note;
            this.Date = DateTime.Parse(date);
            this.DateAdded = DateTime.Parse(dateAdded);
        }

        public void Print()
        {
            Console.WriteLine($"Transaction({this.Date}): R${this.Value}, [{this.Tag}] ... N:{this.Note}");
        }
    }
}
