using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Channel
    {
        public string Name { get; }

        public Channel(string name)
        {
            this.Name = name;
        }
        public List<Transaction> Transactions = new List<Transaction>();

        public void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            Transactions.Add(new Transaction(value, tag, note, date));
        }
        public void Expend(double value, string tag = "", string note = "", string date = "")
        {
            Transactions.Add(new Transaction(-value, tag, note, date));
        }
    }

    public class Transference: Channel
    {
        public Transference(string name) : base(name)
        {
        }
    }

    public class Debit: Channel
    {
        public long Number { get; }

        public Debit(string name, long number) : base(name)
        {
            this.Number = number;
        }
        public void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Credit: Channel
    {
        public long Number { get; }

        public Credit(string name, long number): base(name)
        {
            this.Number = number;
        }
        public void Deposit(double value, string tag = "", string note = "", string date = "")
        {
            throw new NotSupportedException("Not possible to make deposit with debit card");
        }
    }
    public class Pix: Channel
    {
        public Pix(string name) : base(name)
        {
        }

    }
}
