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

            foreach (KeyValuePair<string, Account> item in accounts)
            {
                Console.WriteLine($"Conta: {item.Key}");
                foreach (Transaction t in item.Value.Transactions)
                {
                    Console.WriteLine($"{t.Value} | {t.Note} | {t.Date}");
                }
                Console.WriteLine(" ");
            }




        }
        public static void LoadObjects()
        {
            string dbname = "Arthurpmrs2";
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);


            LoadAccountsCommand command1 = new LoadAccountsCommand(accountHandler);
            Dictionary<string, Account> accounts = command1.Load();
            foreach (KeyValuePair<string, Account> item in accounts)
            {
                Account acc = item.Value;
                Console.WriteLine($"Acc. ID: {acc.ID} | Acc. Name: {acc.Name} | Acc. Bank: {acc.Bank}");
                LoadTransfersCommand command2 = new LoadTransfersCommand(transferHandler, acc);
                Dictionary<string, Transfer> transfers = command2.Load();
                Console.WriteLine(" ");
                Console.WriteLine($"    Transfers:");
                foreach (KeyValuePair<string, Transfer> transferItem in transfers)
                {
                    Transfer t = transferItem.Value;
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

                    }
                    catch (EmptyStorageException e)
                    {
                        Console.WriteLine($"        Transfer >{t.Name}< has no Transaction entries.");
                    }
                }
                Console.WriteLine(" ");
            }
        }
        public static void AddSomeEntries()
        {
            // Adding new entries simulation
            string dbname = "Arthurpmrs2";
            DBHandler accountHandler = DBHandlerFactory.Create(HandlerType.Account, dbname);
            DBHandler transferHandler = DBHandlerFactory.Create(HandlerType.Transfer, dbname);
            DBHandler transactionHandler = DBHandlerFactory.Create(HandlerType.Transaction, dbname);

            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            CreateAccountCommand accountCommand = new CreateAccountCommand(accountHandler);
            accounts.Add("ContaCorrenteBB", accountCommand.Create("ContaCorrenteBB", "BB"));
            accountCommand.GetStoredTransactions(transactionHandler, accounts["ContaCorrenteBB"]);

            accounts.Add("Poupança", accountCommand.Create("Poupança", "CEF"));
            accountCommand.GetStoredTransactions(transactionHandler, accounts["Poupança"]);

            accounts.Add("ContaCorrenteNB", accountCommand.Create("ContaCorrenteNB", "NuBank"));
            accountCommand.GetStoredTransactions(transactionHandler, accounts["ContaCorrenteNB"]);

            CreateTransferCommand transferCommand = new CreateTransferCommand(transferHandler, accounts["ContaCorrenteBB"]);
            transferCommand.Create("Pix", "PixBB", "arthurpmrs@gmail.com");
            transferCommand.Create("Transference", "Conta Corrente", "475454");

            CreateTransferCommand transferCommand2 = new CreateTransferCommand(transferHandler, accounts["Poupança"]);
            transferCommand2.Create("Transference", "Poupança", "475454");

            CreateTransferCommand transferCommand3 = new CreateTransferCommand(transferHandler, accounts["ContaCorrenteNB"]);
            transferCommand3.Create("Pix", "PixNB", "06348234428");

            Console.WriteLine("Transactions");
            foreach (KeyValuePair<string, Account> entry in accounts)
            {
                foreach (Transaction t in entry.Value.Transactions)
                {
                    Console.WriteLine($"{t.Value} | {t.Note}");
                }
            }
            //CreateTransactionCommand transactionCommand1 = new CreateTransactionCommand(transactionHandler,
            //                                                                            accounts["ContaCorrenteBB"],
            //                                                                            accounts["ContaCorrenteBB"].Transfers["PixBB"]);
            //transactionCommand1.Create(-13.90, "Eletronic", "Cabo USB para carregador de celular", "09/09/2021");
            //transactionCommand1.Create(-259.00, "Eletronic", "Chromecast 3", "03/09/2021");
            //transactionCommand1.Create(-233.99, "Eletronic", "Mouse Bluetooth", "20/08/2021");
            //transactionCommand1.Create(-229.90, "Eletronic", "Teclado Bluetooth", "18/08/2021");

            //CreateTransactionCommand transactionCommand2 = new CreateTransactionCommand(
            //    transactionHandler,
            //    accounts["ContaCorrenteBB"],
            //    accounts["ContaCorrenteBB"].Transfers["Conta Corrente"]
            //    );
            //transactionCommand2.Create(1500, "Bolsa", "Bolsa Julho", "05/07/2021");
            //transactionCommand2.Create(1500, "Bolsa", "Bolsa Agosto", "05/08/2021");
            //transactionCommand2.Create(1500, "Bolsa", "Bolsa Setembro", "05/09/2021");
            //transactionCommand2.Create(-990, "Eletronic", "Fones Bluetooth", "25/08/2021");
            //transactionCommand2.Create(250, "Eletronic", "Pagamento fone Bluetooth mãe", "25/08/2021");

            //CreateTransactionCommand transactionCommand3 = new CreateTransactionCommand(
            //    transactionHandler,
            //    accounts["Poupança"],
            //    accounts["Poupança"].Transfers["Transference"]
            //    );
            //transactionCommand3.Create(3000, "Initial", "Budget Inicial", "01/01/2021");
            //transactionCommand3.Create(-1500, "Rent", "Aluguel campinas mais multas", "15/05/2021");

            //CreateTransactionCommand transactionCommand4 = new CreateTransactionCommand(
            //    transactionHandler,
            //    accounts["ContaCorrenteNB"],
            //    accounts["ContaCorrenteNB"].Transfers["PixNB"]
            //    );

            //transactionCommand4.Create(7000, "Initial", "Transferência inicial para pagar notebook", "09/09/2021");
            //transactionCommand4.Create(-64591010, "Eletronic", "Notebook", "09/09/2021");


        }
    }
}
