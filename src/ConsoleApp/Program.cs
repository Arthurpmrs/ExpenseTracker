using System;
using System.Collections.Generic;
using Infrastructure;
using Application.TransferCommands;
using Application.AccountCommands;
using Application.TransactionCommands;
using Application.ApplicationCommands;
using Domain.Entities;
using Domain.Exceptions;
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbname = "Arthurpmrs2";
            ApplicationStarterCommand starterCommand = new ApplicationStarterCommand(dbname);
            Dictionary<string, Account> accounts = starterCommand.Load();

            ShowCommand showCommand = new ShowCommand(accounts);
            showCommand.ShowAllEntries();







            //AddSomeTransactions(dbname, accounts);
        }

        public static void DeleteSome(string dbname, Dictionary<string, Account> accounts)
        {
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);

            Transfer pixBB = accounts["ContaCorrenteBB"].Transfers["PixBB"];

            //DeleteTransactionCommand deleteTransaction = new DeleteTransactionCommand(transactionHandler, accounts["ContaCorrenteBB"],
            //                                                                                              pixBB);
            //deleteTransaction.Delete(pixBB.Transactions[5]);

            //DeleteTransferCommand deleteTransfer = new DeleteTransferCommand(transferHandler, accounts["ContaCorrenteBB"]);
            //deleteTransfer.Delete(pixBB);
        }
        public static void AddSomeTransactions(string dbname, Dictionary<string, Account> accounts)
        {
            // Handlers for db access
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);

            Transfer pixBB = accounts["ContaCorrenteBB"].Transfers["PixBB"];
            Transfer transferenceBB = accounts["ContaCorrenteBB"].Transfers["Conta Corrente"];

            Transfer TransferecneCEF = accounts["Poupança"].Transfers["Poupança"];
            Transfer pixNB = accounts["ContaCorrenteNB"].Transfers["PixNB"];



            CreateTransactionCommand conn_pixBB = new CreateTransactionCommand(transactionHandler, accounts["ContaCorrenteBB"], pixBB);
            conn_pixBB.Create(50, "Avó", "Passar fotos do celular", "20/08/2020");
            conn_pixBB.Create(-236, "Eletronics", "Amazon Fire Stick");
            conn_pixBB.Create(-250, "Eletronics", "Teclado Bluetooth", "25/07/2021");
            conn_pixBB.Create(-212, "Eletronics", "Mouse Bluetooth", "27/07/2021");
            conn_pixBB.Create(1500, "Mestrado", "Bolsa Mês Agosto", "05/08/2021");
            conn_pixBB.Create(1500, "Mestrado", "Bolsa Mês Setembro", "05/09/2021");

            CreateTransactionCommand conn_transferenceBB = new CreateTransactionCommand(transactionHandler, accounts["ContaCorrenteBB"], transferenceBB);
            conn_transferenceBB.Create(-2200, "Aluguel", "Pagamento taxas de quebra de contrato aluguel", "19/05/2021");
            conn_transferenceBB.Create(-700, "Eletronics", "Fones de ouvido");

            CreateTransactionCommand conn_TransferecneCEF = new CreateTransactionCommand(transactionHandler, accounts["Poupança"], TransferecneCEF);
            conn_TransferecneCEF.Create(1900, "Lend", "Pagamento dinheiro emprestado mãe.");
            conn_TransferecneCEF.Create(-20, "Seguro", "Seguro mês 7", "15/07/2021");
            conn_TransferecneCEF.Create(-215, "Eletronics", "ChormeCast", "26/08/2021");

            CreateTransactionCommand conn_pixNB = new CreateTransactionCommand(transactionHandler, accounts["ContaCorrenteNB"], pixNB);
            conn_pixNB.Create(500, "Inicial", "Saldo Inicial");
            conn_pixNB.Create(-78, "Eletronics", "Multímetro Digital");
            conn_pixNB.Create(-38, "Software", "Chave windows 10 amazon");
        }
    }
}
