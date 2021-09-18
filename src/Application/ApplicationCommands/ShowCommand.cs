using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.ApplicationCommands
{
    public class ShowCommand
    {
        private Dictionary<string, Account> Accounts;

        public ShowCommand(Dictionary<string, Account> accounts)
        {
            this.Accounts = accounts;
        }

        public void ShowAllEntries()
        {
            foreach (KeyValuePair<string, Account> acc in this.Accounts)
            {
                Console.WriteLine($"Account: {acc.Value.Name}");
                Console.WriteLine($"------------------------------------------");
                Console.WriteLine(" ");

                foreach (KeyValuePair<string, Transfer> tr in acc.Value.Transfers)
                {
                    Console.WriteLine($"     Transfer: {tr.Value.Name}");

                    foreach (Transaction t in tr.Value.Transactions)
                    {
                        Console.WriteLine($"         Value: {t.Value} | Date: {t.Date} | Note: {t.Note}");
                    }
                    Console.WriteLine(" ");
                }
                Console.WriteLine($"Account Balance: R$ {acc.Value.GetBalance()}");
                //double balance = 0;
                //foreach (Transaction tt in acc.Value.Transactions)
                //{
                //    Console.WriteLine(tt.Value);
                //    balance += tt.Value;
                //}
                Console.WriteLine(" ");
                Console.WriteLine(" ");
            }
        }
    }
}
