using System;
using System.Data;
using System.Collections.Generic;

namespace ExpenseTracker
{
    public class Account
    {
        public string Name { get; }
        public string Bank { get; }
        public long AccountID { get; set; }
        public double Balance
        {
            get
            {
                AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
                List<Tuple<Transaction, Channel>> entrys = wrapper.GetAccountTransactions(this.AccountID);

                double balance = 0;

                foreach (var entry in entrys)
                {
                    balance += entry.Item1.Value;
                }
                return balance;
            }
        }
        private DBHandler DB;

        private Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();

        public Account(DBHandler db, string name, string bank, long accountID = -1)
        {
            this.Name = name;
            this.Bank = bank;
            this.DB = db;
            this.AccountID = accountID;
        }
        public void Save()
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
            this.AccountID = wrapper.InsertAccount(this.Name, this.Bank);
        }

        public Channel AddChannel(string type, string channelName, string identifier = "", bool forceDuplicate = false)
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
            Channel channel;
            try
            {
                channel = GetChannel(channelName);
            }
            catch (InvalidOperationException)
            {
                channel = Factory.Select(this.DB, type, this.AccountID, channelName, identifier);
                channel.Save();
            }
            return channel;
        }

        public Channel GetChannel(string channelName)
        {
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB);
            Channel channel = wrapper.GetChannelByName(channelName);
            
            if (channel != null)
            {
                return channel;
            }
            throw new InvalidOperationException("No such Channel on database.");
        }
        public void ShowChannels()
        {
            ChannelDBWrapper wrapper = new ChannelDBWrapper(this.DB);
            List<Channel> channels = wrapper.GetAllChannels(this.AccountID);
            if (channels != null)
            {
                string headerTitle = $"Channels for Account: {this.Name} - ID: {this.AccountID}";
                Console.WriteLine(" ");
                Console.WriteLine(new string('-', headerTitle.Length));
                Console.WriteLine(headerTitle);
                Console.WriteLine(new string('-', headerTitle.Length));
                foreach (Channel channel in channels)
                {
                    Console.WriteLine($"Ch. ID: {channel.ChannelID,4} | Type: {channel.Type,12} | Ch. Name: {channel.Name,16} | Ch. Nº: {channel.Identifier, 16}");
                }
            }
        }
        public void History()
        {
            AccountDBWrapper wrapper = new AccountDBWrapper(this.DB);
            List<Tuple<Transaction, Channel>> transactions = wrapper.GetAccountTransactions(this.AccountID);
            if (transactions != null)
            {
                string headerTitle = $"Transactions for Account: {this.Name} - ID: {this.AccountID}";
                Console.WriteLine(" ");
                Console.WriteLine(new string('-', headerTitle.Length));
                Console.WriteLine(headerTitle);
                Console.WriteLine(new string('-', headerTitle.Length));
                foreach (var item in transactions)
                {
                    Transaction t = item.Item1;
                    Channel c = item.Item2;
                    string[] fields = new[] { 
                        $"TransactionID: {t.TransactionID,4}",
                        $"Value: {t.Value,8}",
                        $"Tag: {t.Tag, 10}",
                        $"Channel: {c.Type, 15}",
                        $"ChName: {c.Name, 16}",
                    }; 
                    Console.WriteLine(String.Join(" | ", fields));
                }
            }
            else
            {
                Console.WriteLine("No Transaction has been added!");
            }
        }
    }
}
