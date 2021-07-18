using System;
using System.Collections.Generic;

namespace ExpenseTracker
{
    public class Account
    {
        public string Card { get; }
        public double Balance { 
            get
            {
                double total = 0;

                foreach (double transaction in Transactions) {
                    total += transaction;
                }
                return total;
            }
        }
        private List<double> Transactions = new List<double>();

        public Account(string card, double initialTransaction)
        {
            this.Card = card;
            Transactions.Add(initialTransaction);
        }
        public void MakeDeposit(double value)
        {
            Transactions.Add(value);
        }
        public void History()
        {
            foreach (double transaction in Transactions)
            {
                Console.WriteLine($"Value: R${transaction}");
                Console.WriteLine($"Total: R${this.Balance}");
            }
        }
    }
}
