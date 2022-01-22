using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;
using Domain.Exceptions;
using Application.AccountCommands;
using Application.TransferCommands;
using Application.TransactionCommands;


namespace Application.ApplicationCommands
{
    public class ApplicationStarterCommand
    {
        public string DBName { get; set; }
        public ApplicationStarterCommand(string dbname)
        {
            this.DBName = dbname;
        }

        public Dictionary<string, Account> Load()
        {
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, this.DBName);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, this.DBName);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, this.DBName);

            Dictionary<string, Account> accounts = new Dictionary<string, Account>();

            // Load all accounts from DB
            LoadAccountsCommand accountCommand = new LoadAccountsCommand(accountHandler);
            try
            {
                accounts = accountCommand.Load();
            } catch (EmptyStorageException)
            {
                Console.WriteLine("There are no Accounts. Create One.");
            }
            
            foreach (KeyValuePair<string, Account> accountEntry in accounts)
            {
                Account acc = accountEntry.Value;
                Dictionary<string, Transfer> transfers = new Dictionary<string, Transfer>();

                LoadTransfersCommand transferCommand = new LoadTransfersCommand(transferHandler, acc);
                try
                {
                     transfers = transferCommand.Load();
                } catch (EmptyStorageException)
                {
                    Console.WriteLine($"There are no Transfers in <{acc.Name}> account. Add One.");
                }

                LoadTransactionsCommand accountTransactionCommand = new LoadTransactionsCommand(transactionHandler, acc);

                try
                {
                    accountTransactionCommand.LoadAccountTransactions();
                } catch (EmptyStorageException)
                {
                    Console.WriteLine($"There are no Transactions in <{acc.Name}> account. Add one");
                }


                foreach (KeyValuePair<string, Transfer> transferEntry in transfers)
                {
                    Transfer t = transferEntry.Value;
                    List<Transaction> transactions = new List<Transaction>();

                    LoadTransactionsCommand transferTransactionCommand = new LoadTransactionsCommand(transactionHandler, acc, t);
                    try
                    {
                        transferTransactionCommand.LoadTransferTransactions();
                    } catch (EmptyStorageException)
                    {
                        Console.WriteLine($"There are no Transactions in <{t.Name}> transfer. Add one");
                    }
                    
                }
            }
            return accounts;
        }
    }
}
