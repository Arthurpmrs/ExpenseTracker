using System;
using System.Collections.Generic;
using System.Data;

namespace ExpenseTracker
{
    public class Profile
    {
        public string Name { get; }

        private DBHandler DB;

        public Profile(string name)
        {
            this.Name = name;
            this.DB = new DBHandler(name);
        }

        public void AddAccount(string accountName, string bankName, bool forceDuplicate = false)
        {
            ProfileDBWrapper wrapper = new ProfileDBWrapper(this.DB.connectionString);
            if (GetAccount(accountName) == null || forceDuplicate == true)
            {
                wrapper.InsertAccount(accountName, bankName);
            } else
            {
                throw new DuplicateNameException("Account already created. Use forceDuplicate to create anyways.");
            }
        }

        public Account GetAccount(string accountName)
        {
            ProfileDBWrapper wrapper = new ProfileDBWrapper(this.DB.connectionString);
            dynamic accountFields = wrapper.GetAccountByName(accountName);

            if (accountFields != null)
            {
                return new Account(this.DB, accountFields.RowID, accountFields.Name, accountFields.Bank);
            }
            throw new InvalidOperationException("No such Account on database.");
        }

        public void ShowAccounts()
        {
            ProfileDBWrapper wrapper = new ProfileDBWrapper(this.DB.connectionString);
            List<dynamic> accounts = wrapper.GetAllAccounts();
            if (accounts != null)
            {
                foreach (dynamic account in accounts)
                {
                    Console.WriteLine($"Acc. ID: {account.RowID, 4} | Bank: {account.Bank, 5} | Acc. Name: {account.Name,12}");
                }
            } else
            {
                Console.WriteLine("No Account has been added!");
            }
        }
    }
}
