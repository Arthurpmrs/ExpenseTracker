using System;
using System.Collections.Generic;
using Infrastructure;
using Application;
using Domain.Entities;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbname = "Arthurpmrs2";
            DBHandler accHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);

            CreateAccountCommand Command = new CreateAccountCommand(accHandler);
            Account acc1 = Command.Create("ContaCorrenteBB", "BB");

            DBHandler trnsHandler1 = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            CreateTransferCommand Command2 = new CreateTransferCommand(trnsHandler1, acc1);
            Transfer trns1 = Command2.Create("Pix", "PixBB", "arthurpmrs@gmail.com");

            Transfer trans2 = Command2.Create("Transference", "Conta Corrente", "78454 548785-0");

            Account acc2 = Command.Create("Poupança", "CEF");
            CreateTransferCommand Command3 = new CreateTransferCommand(trnsHandler1, acc2);
            Transfer trns2 = Command3.Create("Transference", "Poupança", "78951 74157458");

            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);
            CreateTransactionCommand Command4 = new CreateTransactionCommand(transactionHandler, acc1, trns1);
            Transaction t1 = Command4.Create(178, "Natal", "Presente da minha avó", "2020-12-20");
            Transaction t2 = Command4.Create(1500, "Bolsa", "Bolsa do mestrado", "2021-08-05");

            foreach (Transaction t in acc1.Transactions) 
            {
                Console.WriteLine($"Value: {t.Value} | Date: {t.Date}");
            }
            Console.WriteLine("---------");
            foreach (Transaction t in trns1.Transactions)
            {
                Console.WriteLine($"Value: {t.Value} | Date: {t.Date}");
            }

        }
    }
}
