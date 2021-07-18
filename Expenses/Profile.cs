using System;
using System.Collections.Generic;

namespace ExpenseTracker
{
    public class Profile
    {
        public string Name { get; }

        private Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

        public Profile(string name)
        {
            this.Name = name;
        }

        public void AddAccount(string accountName, string bankName)
        {
            Accounts.Add(accountName, new Account(accountName, bankName));
        }
        public Account getAccount(string accountName)
        {
            return Accounts[accountName];
        }
    }
}
