using System;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Profile myProfile = new Profile("Arthurpmrs");

            string accountName = "Poupança Caixa";

            myProfile.AddAccount(accountName, "CEF", 100);

            Account myAccount1 = myProfile.getAccount(accountName);
            myAccount1.Deposit(100.50, "Pix", "Presente");
            myAccount1.Expend(21.50, "Debit", "Sanduiche");
            myAccount1.Deposit(1500, "Deposit", "Bolsa Mestrado");
            myAccount1.Expend(300, "Credit", "HD Externo", "20/06/2021");
            myAccount1.Deposit(500, "Deposit", "Presene de natal avó", "24/12/2020");
            myAccount1.Expend(1200, "Debit", "Bicicleta");            
            myAccount1.History();

        }
    }
}
