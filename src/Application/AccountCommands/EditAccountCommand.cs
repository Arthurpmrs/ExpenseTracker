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
                field.AccountName = newName;
                account.Name = newName;
            }
            if (newBank != null)
            {
                field.BankName = newBank;
                account.Bank = newBank;
            }

            this.Handler.EditByID(account.ID, field);

            this.Accounts.Remove(accountKey);
            this.Accounts.Add(account.Name, account);
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
