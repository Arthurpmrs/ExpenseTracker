using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;
using Domain.Helpers;

namespace Application.AccountCommands
{
    public class DeleteAccountCommand
    {
        private DBHandler Handler;
        private Dictionary<string, Account> Accounts;

        public DeleteAccountCommand(DBHandler handler, Dictionary<string, Account> accounts)
        {
            this.Handler = handler;
            this.Accounts = accounts;
        }

        public void Delete(Account account)
        {
            Fields field = new Fields
            {
                AccountID = account.ID
            };
            this.Handler.DeleteBy(field);
            foreach(KeyValuePair<string, Account> acc in this.Accounts)
            {
                if(acc.Value == account)
                {
                    this.Accounts.Remove(acc.Key);
                }
            }
        }
    }
}
