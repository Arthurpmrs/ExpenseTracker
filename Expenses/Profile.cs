using System;
using System.Collections.Generic;
using System.Data;

namespace ExpenseTracker
{
    public class Profile
    {
        public string Name { get; }

        public DBHandler DB { get; }

        public Profile(string name)
        {
            this.Name = name;
            this.DB = new DBHandler(name);
        }

        public Account AddAccount(string accountName, string bankName)
        {
            ProfileDBWrapper wrapper = new ProfileDBWrapper(this.DB.connectionString);
            Account account;
            try
            {
                account = GetAccount(accountName);
            } 
            catch (InvalidOperationException)
            {
                account = new Account(this.DB, accountName, bankName);
                account.Save();
            }
            return account;
        }

        public Account GetAccount(string accountName)
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
            Account account = wrapper.GetAccountByName(accountName);

            if (account != null)
            {
                return account;
            }
            throw new InvalidOperationException("No such Account on database.");
        }

        public void ShowAccounts()
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
            List<Account> accounts = wrapper.GetAllAccounts();
            if (accounts != null)
            {
                string headerTitle = $"Accounts for Profile: {this.Name}";
                Console.WriteLine(" ");
                Console.WriteLine(new string('-', headerTitle.Length));
                Console.WriteLine(headerTitle);
                Console.WriteLine(new string('-', headerTitle.Length));
                foreach (Account account in accounts)
                {
                    Console.WriteLine($"Acc. ID: {account.AccountID, 4} | Bank: {account.Bank, 5} | Acc. Name: {account.Name,12}");
                }
            } else
            {
                throw new InvalidOperationException("No Account added to database.");
            }
        }
    }
}
