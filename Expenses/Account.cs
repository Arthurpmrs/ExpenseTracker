using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Account
    {
        public string Name { get; }
        public string Bank { get; }
        public double Balance { 
            get
            {
                double balance = 0;

                foreach (Transaction transaction in Transactions)
                {
                    balance += transaction.Value;
                }
                return balance;
            } 
        }

        private List<Transaction> Transactions = new List<Transaction>();

        public Account(string name, string bank, double firstDeposit)
        {
            this.Name = name;
            this.Bank = bank;
            Deposit(firstDeposit, "Deposit", "Initial Deposit", "");
        }

        public void Deposit(double value, string channel, string note, string date = "")
        {
            Transactions.Add(new Transaction(value, channel, note, date));
        }

        public void Expend(double value, string channel, string note, string date = "")
        {
            Transactions.Add(new Transaction(-value, channel, note, date));
        }

        public void History()
        {
            foreach (Transaction t in Transactions)
            {
                if (t.Value > 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Value: R${t.Value} | Channel: {t.Channel} | Date: {t.Date.ToString("yyyy/MM/dd")} | Note: {t.Note}");
                } else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Value: R${-t.Value} | Channel: {t.Channel} | Date: {t.Date.ToString("yyyy/MM/dd")} | Note: {t.Note}");
                }
                Console.ResetColor();
            }
            Console.WriteLine("######################");
            Console.WriteLine($"Total Balance: R${this.Balance}");
        }
    }
}
