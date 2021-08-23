using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;


namespace Application.AccountCommands
{
    public class CreateAccountCommand
    {
        private DBHandler Handler;

        public CreateAccountCommand(DBHandler handler)
        {
            this.Handler = handler;
        }
        public Account Create(string name, string bank)
        {
            Account account;
            Fields fieldsFromDB = this.Handler.GetByName(name);
            if (fieldsFromDB != null)
            {
                account = new Account(
                    fieldsFromDB.AccountID, 
                    fieldsFromDB.AccountName, 
                    fieldsFromDB.BankName
                    );
                Console.WriteLine($"Account {name} already exists.");
            } else
            {
                Fields fields = new Fields()
                {
                    AccountName = name,
                    BankName = bank
                };
                long ID = this.Handler.Insert(fields);
                account = new Account(ID, name, bank);
            }
            return account;
        }

    }
}
