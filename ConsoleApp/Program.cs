using System;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Profile myProfile = new Profile("Arthurpmrs");

            string accountName = "Poupança Caixa";

            myProfile.AddAccount(accountName, "CEF");

            Account myAccount1 = myProfile.getAccount(accountName);
            myAccount1.AddChannel("Debit", "DebitoElo", 45214754855458);

            //myAccount1.TesteMethod();

            Channel debitCard = myAccount1.GetChannel("DebitoElo");
            Channel transference = myAccount1.GetChannel("transference");

            transference.Deposit(100.50, "Gift");
            debitCard.Expend(21.50, "Food", "Sanduiche");
            transference.Deposit(1500, "Bolsa", "Bolsa Mestrado");
            debitCard.Expend(300, "Amazon", "HD Externo", "20/06/2021");
            transference.Deposit(500, "Gift", "Presene de natal avó", "24/12/2020");
            debitCard.Expend(1200, "Amazon", "Bicicleta");
            debitCard.Expend(10, "Food", "Morangos");
            myAccount1.History();

        }
    }
}
