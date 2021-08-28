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









            //DBHandler transHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);
            //LoadTransactionsCommand Command2 = new LoadTransactionsCommand(transHandler);
            //List<Transaction> trans = Command2.Load();



            //int i = 0;
            //foreach (Transaction t in trans)
            //{
            //    Console.WriteLine($"i: {i} | Value: {t.Value} | AccountID {t.AccountID}");
            //    i++;
            //}
            //for (int i = 0; i < trans.Count; i++)
            //{
            //    Console.WriteLine($"i: {i} | Value: {trans[i].Value} | AccID {trans[i].AccountID} | {trans[i].Note}");
            //}
            //DeleteTransactionCommand DelCommand = new DeleteTransactionCommand(transHandler);
            //DelCommand.Delete(trans[0]);

            //List<Transaction> trans1 = Command2.Load();
            //for (int i = 0; i < trans1.Count; i++)
            //{
            //    Console.WriteLine($"i: {i} | Value: {trans1[i].Value} | AccID {trans1[i].AccountID} | {trans1[i].Note}");
            //}

            //CreateAccountCommand Command = new CreateAccountCommand(accHandler);
            //Account acc1 = Command.Create("ContaCorrenteBB", "BB");

            //DBHandler trnsHandler1 = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            //CreateTransferCommand Command2 = new CreateTransferCommand(trnsHandler1, acc1);
            //Transfer trns1 = Command2.Create("Pix", "PixBB", "arthurpmrs@gmail.com");

            //Transfer trans2 = Command2.Create("Transference", "Conta Corrente", "78454 548785-0");

            //Account acc2 = Command.Create("Poupança", "CEF");
            //CreateTransferCommand Command3 = new CreateTransferCommand(trnsHandler1, acc2);
            //Transfer trns2 = Command3.Create("Transference", "Poupança", "78951 74157458");

            //DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);
            //CreateTransactionCommand Command4 = new CreateTransactionCommand(transactionHandler, acc1, trns1);
            //Transaction t1 = Command4.Create(178, "Natal", "Presente da minha avó", "2020-12-20");
            //Transaction t2 = Command4.Create(1500, "Bolsa", "Bolsa do mestrado", "2021-08-05");

            //foreach (Transaction t in acc1.Transactions) 
            //{
            //    Console.WriteLine($"Value: {t.Value} | Date: {t.Date}");
            //}
            //Console.WriteLine("---------");
            //foreach (Transaction t in trns1.Transactions)
            //{
            //    Console.WriteLine($"Value: {t.Value} | Date: {t.Date}");
            //}

        }
    }
}
