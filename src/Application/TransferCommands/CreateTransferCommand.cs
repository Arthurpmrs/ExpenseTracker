using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.TransferCommands
{
    public class CreateTransferCommand
    {
        private DBHandler Handler;
        private Account Account;
        public CreateTransferCommand(DBHandler handler, Account account)
        {
            this.Handler = handler;
            this.Account = account;
        }
        public Transfer Create(string type, string name, string identifier)
        {
            Transfer transfer;
            Fields fieldsFromDB = this.Handler.GetByName(name);
            if (fieldsFromDB != null)
            {
                Enum.TryParse(fieldsFromDB.TransferType, out TransferType transferTypeFromDB);
                transfer = TransferFactory.Create(
                    transferTypeFromDB,
                    fieldsFromDB.TransferID,
                    fieldsFromDB.AccountID,
                    fieldsFromDB.TransferName,
                    fieldsFromDB.TransferIdentifier
                    );
                Console.WriteLine($"Transfer Method >{name}< in already exists in Account {this.Account.Name}.");
            } else
            {
                Fields fields = new Fields()
                {
                    TransferType = type,
                    TransferIdentifier = identifier,
                    TransferName = name,
                    AccountID = this.Account.ID
                };
                long ID = this.Handler.Insert(fields);

                Enum.TryParse(type, out TransferType transferType);
                Console.WriteLine(transferType);

                transfer = TransferFactory.Create(transferType, ID, this.Account.ID, name, identifier);
                this.Account.Transfers.Add(name, transfer);
            }
            return transfer;
        }
    }
}
