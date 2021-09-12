using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.AccountCommands
{
    public class LoadAccountsCommand
    {
        private DBHandler Handler;
        public LoadAccountsCommand(DBHandler handler)
        {
            this.Handler = handler;
        }
        public Dictionary<string, Account> Load()
        {
            Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

            List<Fields> accountsFields = this.Handler.GetAll();
            if (accountsFields == null)
            {
                throw new EmptyStorageException("There is no account. Create One.");
            } else
            {
                foreach (Fields f in accountsFields)
                {
                    Accounts.Add(
                        f.AccountName,
                        new Account(f.AccountID, f.AccountName, f.BankName)
                        );
                }
            }
            
            return Accounts;
        }
    }
}
