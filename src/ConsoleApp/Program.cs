using System;
using System.Collections.Generic;
using Infrastructure;
using Application.TransferCommands;
using Application.AccountCommands;
using Application.TransactionCommands;
using Domain.Entities;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbname = "Arthurpmrs2";
            DBHandler accHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);

            LoadAccountsCommand Command = new LoadAccountsCommand(accHandler);
            List<Account> acc = Command.Load();

            foreach (Account a in acc)
            {
                Console.WriteLine($"Acc. ID: {a.ID} | Acc. Name: {a.Name} | Acc. Bank: {a.Bank}");
            }

            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            LoadTransfersCommand Command2 = new LoadTransfersCommand(transferHandler, acc[0]);
            List<Transfer> transfers = Command2.Load();


            foreach (Transfer t in transfers)
            {
                Console.WriteLine($"ID: {t.ID} | Nome: {t.Name} | Type: {t.Type} | AccName: {acc.Find(x => x.ID == t.AccountID).Name}");
            }

            LoadTransfersCommand Command3 = new LoadTransfersCommand(transferHandler, acc[1]);
            List<Transfer> transfers2 = Command3.Load();

            foreach (Transfer t1 in transfers2)
            {
                Console.WriteLine($"ID: {t1.ID} | Nome: {t1.Name} | Type: {t1.Type} | AccName: {acc.Find(x => x.ID == t1.AccountID).Name}");
            }


            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);

            LoadTransactionsCommand Command4 = new LoadTransactionsCommand(transactionHandler, acc[0], transfers[0]);
            List<Transaction> transactions = Command4.LoadAccountTransactions();
            Console.WriteLine("-----------------------");
            foreach (Transaction tr in transactions)
            {
                //Console.WriteLine($"Value: R${tr.Value} | Transfer: {transfers.Find(x => x.ID == tr.TransferID).Type}");
                Console.WriteLine($"Value: R${tr.Value} | Transfer: {tr.TransferID}");
            }

        }
    }
}
