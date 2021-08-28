﻿using System;
using System.Collections.Generic;
using Infrastructure;
using Application;
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

            DBHandler transHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);
            LoadTransactionsCommand Command2 = new LoadTransactionsCommand(transHandler);
            List<Transaction> trans = Command2.Load();



            //int i = 0;
            //foreach (Transaction t in trans)
            //{
            //    Console.WriteLine($"i: {i} | Value: {t.Value} | AccountID {t.AccountID}");
            //    i++;
            //}
            for (int i = 0; i < trans.Count; i++)
            {
                Console.WriteLine($"i: {i} | Value: {trans[i].Value} | AccID {trans[i].AccountID} | {trans[i].Note}");
            }
            DeleteTransactionCommand DelCommand = new DeleteTransactionCommand(transHandler);
            DelCommand.Delete(trans[0]);

            List<Transaction> trans1 = Command2.Load();
            for (int i = 0; i < trans1.Count; i++)
            {
                Console.WriteLine($"i: {i} | Value: {trans1[i].Value} | AccID {trans1[i].AccountID} | {trans1[i].Note}");
            }

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
