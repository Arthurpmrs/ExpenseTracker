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

                foreach (var item in Channels)
                {
                    var channel = item.Value.Transactions;
                    foreach (Transaction transaction in channel)
                    {
                        balance += transaction.Value;
                    }
                }
                return balance;
            } 
        }

        private Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();

        public Account(string name, string bank)
        {
            this.Name = name;
            this.Bank = bank;
            Channels.Add("transference", new Transference("Base"));
        }

        public void AddChannel(string type, string name, long number = 0)
        {
            if (type == "Debit")
            {
                Debit debitCard = new Debit(name, number);
                Channels.Add(name, debitCard);
            } else if (type == "Credit")
            {
                Credit creditCard = new Credit(name, number);
                Channels.Add(name, creditCard);
            } else
            {
                Console.WriteLine("Oops!");
            }
        }
        public Channel GetChannel(string channelName)
        {
            return Channels[channelName];
        }
        public void History()
        {
            foreach (var item in Channels)
            {
                var channel = item.Value;
                foreach (Transaction t in channel.Transactions)
                {
                    if (t.Value > 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Value: R${t.Value} | Channel: {channel.GetType().Name} | Date: {t.Date.ToString("yyyy/MM/dd")} | Note: {t.Note}");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Value: R${-t.Value} | Channel: {channel.GetType().Name} | Date: {t.Date.ToString("yyyy/MM/dd")} | Note: {t.Note}");
                    }
                    Console.ResetColor();
                }
            }
            
            Console.WriteLine("######################");
            Console.WriteLine($"Total Balance: R${this.Balance}");
        }
    }
}
