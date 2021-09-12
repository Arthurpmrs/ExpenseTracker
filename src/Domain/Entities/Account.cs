using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Bank { get; set; }
        public double Balance
        {
            get
            {
                double balance = 0;
                foreach (var t in Transactions)
                {
                    balance += t.Value;
                }
                return Balance;
            }
        }

        public Dictionary<string, Transfer> Transfers { get; private set; } = new Dictionary<string, Transfer>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Account(long id, string name, string bank)
        {
            this.ID = id;
            this.Name = name;
            this.Bank = bank;
        }
        public void AddTransfer(Transfer transfer)
        {
            this.Transfers.Add(transfer.Name, transfer);
        }
    }
}
