using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.AccountCommands
{
    public class EditAccountCommand
    {
        private DBHandler Handler;
        private Dictionary<string, Account> Accounts;

        public EditAccountCommand(DBHandler handler, Dictionary<string, Account> accounts)
        {
            this.Handler = handler;
            this.Accounts = accounts;
        }
        public void Edit(Account account, string newName = null, string newBank = null)
        {
            if (newName == null && newBank == null)
            {
                throw new ArgumentNullException("Bank or Name must be added to edit a Account.");
            }

            string accountKey = GetAccountKey(account);
            Fields field = new Fields();
            if (newName != null)
            {
                account.Name = newName;
                field.AccountName = newName;
            }
            if (newBank != null)
            {
                account.Bank = newBank;
                field.BankName = newBank;
            }
           
            this.Handler.EditByID(account.ID, field);

            this.Accounts.Remove(accountKey);
            if(newName == null)
            {
                this.Accounts.Add(accountKey, account);
            } else
            {
                this.Accounts.Add(newName, account);
            }

        }

        private string GetAccountKey(Account account)
        {
            foreach(KeyValuePair<string, Account> acc in this.Accounts)
            {
                if (acc.Value == account)
                {
                    return acc.Key;
                }
            }
            return null;
        }
    }
}
