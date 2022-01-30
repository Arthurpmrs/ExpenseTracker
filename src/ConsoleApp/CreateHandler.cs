using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure;
using Application.ApplicationCommands;
using Application.AccountCommands;
using Application.TransferCommands;
using Application.TransactionCommands;

namespace ConsoleApp
{
    internal class CreateHandler
    {
        public static void CreateAccount(string name, string bank)
        {
            string dbname = ConfigFileHandler.GetDBName();
            DBHandler AccountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            CreateAccountCommand createAccountCommand = new CreateAccountCommand(AccountHandler);
            _ = createAccountCommand.Create(name, bank);
            Console.WriteLine($"Conta {name} criada com sucesso.");
        }
        public static void CreateTransfer(string accountName, string type, string name, string identifier)
        {
            string dbname = ConfigFileHandler.GetDBName();
            DBHandler AccountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            LoadAccountsCommand loadAccountCmd = new LoadAccountsCommand(AccountHandler);
            Account account = loadAccountCmd.LoadAccountByName(accountName);

            DBHandler TransferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            CreateTransferCommand createTransferCommand = new CreateTransferCommand(TransferHandler, account);
            _ = createTransferCommand.Create(type, name, identifier);

            Console.WriteLine($"Transfer {name} foi criado com sucesso!");
        }
        public static void Expense(string accountName, string transferName, double value, string tags, string note, string date)
        {
            CreateTransactionCommand createTransactionCommand = _SetupTransactionCommand(accountName, transferName);
            createTransactionCommand.Create(-value, tags, note, date);

            Console.WriteLine($"Expense de R${value} registrado.");
        }
        public static void Income(string accountName, string transferName, double value, string tags, string note, string date)
        {
            CreateTransactionCommand createTransactionCommand = _SetupTransactionCommand(accountName, transferName);
            createTransactionCommand.Create(value, tags, note, date);

            Console.WriteLine($"Income de R${value} registrado.");
        }

        private static CreateTransactionCommand _SetupTransactionCommand(string accountName, string transferName)
        {
            string dbname = ConfigFileHandler.GetDBName();
            DBHandler AccountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            LoadAccountsCommand loadAccountsCommand = new LoadAccountsCommand(AccountHandler);
            Account account = loadAccountsCommand.LoadAccountByName(accountName);

            DBHandler TransferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            LoadTransfersCommand loadTransferCommmand = new LoadTransfersCommand(TransferHandler, account);
            Transfer transfer = loadTransferCommmand.LoadTransferByName(transferName);

            DBHandler TransactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);
            CreateTransactionCommand createTransactionCommand = new CreateTransactionCommand(TransactionHandler, account, transfer);
            return createTransactionCommand;
        }
    }
}
