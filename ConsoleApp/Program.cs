using System;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new Account("Banco do Brasil", 150);
            account.MakeDeposit(120);
            account.MakeDeposit(-30);
            account.MakeDeposit(1500);
            account.MakeDeposit(-433);
            account.History();
        }
    }
}
