using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.AccountCommands
{
    public class LoadAccountsCommand
    {
        private DBHandler Handler;
        public LoadAccountsCommand(DBHandler handler)
        {
            this.Handler = handler;
        }
        public List<Account> Load()
        {
            List<Account> Accounts = new List<Account>();

            List<Fields> accountsFields = this.Handler.GetAll();

            foreach (Fields f in accountsFields)
            {
                Accounts.Add(
                    new Account(f.AccountID, f.AccountName, f.BankName)
                    );
            }
            return Accounts;
        }
    }
}
