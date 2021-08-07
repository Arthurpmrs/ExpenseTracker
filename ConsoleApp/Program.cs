using System;
using System.Data;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Profile myProfile = new Profile("Arthurpmrs");

            string accountName1 = "Poupança Caixa";
            string accountName2 = "Conta Corrente BB";

            Account myAccount1 = myProfile.AddAccount(accountName1, "CEF");
            Account myAccount2 = myProfile.AddAccount(accountName2, "BB");

            myProfile.ShowAccounts();

            Channel transference = myAccount1.AddChannel("Transference", "TransaçãoComum");
            Channel debitCard = myAccount1.AddChannel("Debit", "DebitoElo", "45214754855458");
            Channel pix = myAccount2.AddChannel("Pix", "PixBB", "arthurpmrs@gmail.com");

            myAccount1.ShowChannels();
            myAccount2.ShowChannels();

            pix.Deposit(1459, "borrowed", "Passagem BH mãe", "29/07/2021");
            pix.Expend(700, "move", "Empresa de mudança", "14/03/2021");
            pix.Expend(60, "borrowed", "contadora");
            debitCard.Expend(120, "Amazon", "Cases HD Externo", "20/10/2020");
            transference.Deposit(100.50, "Gift");
            debitCard.Expend(21.50, "Food", "Sanduiche");
            transference.Deposit(1500, "Bolsa", "Bolsa Mestrado");
            debitCard.Expend(300, "Amazon", "HD Externo", "20/06/2021");
            transference.Deposit(500, "Gift", "Presene de natal avó", "24/12/2020");
            debitCard.Expend(1200, "Amazon", "Bicicleta");
            debitCard.Expend(10, "Food", "Morangos");
            myAccount1.History();
            double balance = myAccount1.Balance;
            Console.WriteLine($"Total Balance: R$ {balance}");
            myAccount2.History();
            double balance2 = myAccount2.Balance;
            Console.WriteLine($"Total Balance: R$ {balance2}");
            Console.WriteLine(" ");

            Transaction transaction1 = debitCard.GetTransaction(5);
            transaction1.Print();

        }
    }
}
