using System;
using System.Collections.Generic;
using Infrastructure;
using Application.TransferCommands;
using Application.AccountCommands;
using Application.TransactionCommands;
using Domain.Entities;
using Domain.Exceptions;
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbname = "Arthurpmrs2";
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);


            LoadAccountsCommand command1 = new LoadAccountsCommand(accountHandler);
            List<Account> accounts = command1.Load();
            foreach (Account acc in accounts)
            {
                Console.WriteLine($"Acc. ID: {acc.ID} | Acc. Name: {acc.Name} | Acc. Bank: {acc.Bank}");
                LoadTransfersCommand command2 = new LoadTransfersCommand(transferHandler, acc);
                List<Transfer> transfers = command2.Load();
                Console.WriteLine(" ");
                Console.WriteLine($"    Transfers:");
                foreach (Transfer t in transfers)
                {
                    Console.WriteLine($"    ID: {t.ID} | Nome: {t.Name} | Type: {t.Type}");
                    LoadTransactionsCommand command3 = new LoadTransactionsCommand(transactionHandler, acc, t);
                    Console.WriteLine($"        Transactions:");
                    try
                    {
                        List<Transaction> transactions = command3.LoadTransferTransactions();
                        foreach (Transaction tr in transactions)
                        {
                            Console.WriteLine($"        Value: R${tr.Value} | Transfer: {tr.TransferID}");
                        }
                        Console.WriteLine(" ");

                    } catch (EmptyStorageException e)
                    {
                        Console.WriteLine($"        Transfer >{t.Name}< has no Transaction entries.");
                    }
                }
                Console.WriteLine(" ");
            }

            //foreach (Account a in acc)
            //{
            //    Console.WriteLine($"Acc. ID: {a.ID} | Acc. Name: {a.Name} | Acc. Bank: {a.Bank}");
            //}

           
            //LoadTransfersCommand Command2 = new LoadTransfersCommand(transferHandler, acc[0]);
            ////List<Transfer> transfers = Command2.Load();


            ////foreach (Transfer t in transfers)
            ////{
            ////    Console.WriteLine($"ID: {t.ID} | Nome: {t.Name} | Type: {t.Type} | AccName: {acc.Find(x => x.ID == t.AccountID).Name}");
            ////}

            //LoadTransfersCommand Command3 = new LoadTransfersCommand(transferHandler, acc[1]);
            //List<Transfer> transfers2 = Command3.Load();

            //foreach (Transfer t1 in transfers2)
            //{
            //    Console.WriteLine($"ID: {t1.ID} | Nome: {t1.Name} | Type: {t1.Type} | AccName: {acc.Find(x => x.ID == t1.AccountID).Name}");
            //}



            //LoadTransactionsCommand Command4 = new LoadTransactionsCommand(transactionHandler, acc[0], transfers[0]);
            ////List<Transaction> transactions = Command4.LoadAccountTransactions();
            //Console.WriteLine("-----------------------");
            //foreach (Transaction tr in transactions)
            //{
            //    //Console.WriteLine($"Value: R${tr.Value} | Transfer: {transfers.Find(x => x.ID == tr.TransferID).Type}");
            //    Console.WriteLine($"Value: R${tr.Value} | Transfer: {tr.TransferID}");
            //}

        }
    }
}
