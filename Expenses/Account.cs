using System;
using System.Data;
using System.Collections.Generic;

namespace ExpenseTracker
{
    public class Account
    {
        public string Name { get; }
        public string Bank { get; }
        public int AccountID { get; }
        public double Balance
        {
            get
            {
                AccountDBWrapper wrapper = new AccountDBWrapper(this.DB.connectionString);
                List<dynamic> transactions = wrapper.GetAccountTransactions(this.AccountID);

                double balance = 0;

                foreach (var item in transactions)
                {
                    balance += item.Value;
                }
                return balance;
            }
        }
        private DBHandler DB;

        private Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();

        public Account(DBHandler db, int accountID, string name, string bank)
        {
            this.Name = name;
            this.Bank = bank;
            this.AccountID = accountID;
            this.DB = db;
        }

        public void AddChannel(string type, string channelName, string identifier = "", bool forceDuplicate = false)
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB.connectionString);
            if (GetChannel(channelName) == null || forceDuplicate == true)
            {
                wrapper.InsertCannel(type, channelName, identifier, this.AccountID);
            }
            else
            {
                throw new DuplicateNameException("Channel already created. Use forceDuplicate to create anyways.");
            }
        }

        public Channel GetChannel(string channelName)
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB.connectionString);
            dynamic channelFields = wrapper.GetChannelByName(channelName);
            
            if (channelFields != null)
            {
                return Factory.Select(
                    this.DB,
                    channelFields.RowID,
                    channelFields.Type,
                    channelFields.AccountID,
                    channelFields.Name,
                    channelFields.Identifier
                    );
            }
            throw new InvalidOperationException("No such Channel on database.");
        }
        public void History()
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB.connectionString);
            List<dynamic> transactions = wrapper.GetAccountTransactions(this.AccountID);
            if (transactions != null)
            {
                string headerTitle = $"Transactions for Account: {this.Name} - ID: {this.AccountID}";
                Console.WriteLine(" ");
                Console.WriteLine(new string('-', headerTitle.Length));
                Console.WriteLine(headerTitle);
                Console.WriteLine(new string('-', headerTitle.Length));
                foreach (dynamic t in transactions)
                {
                    Console.WriteLine($"TransactionID: {t.RowID,4} | Value: {t.Value,8} | Channel: {t.ChannelType, 15} | ChName: {t.ChannelName, 12}");
                }
            }
            else
            {
                Console.WriteLine("No Transaction has been added!");
            }
        }
    }
}
