using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.TransferCommands
{
    public class EditTransferCommand
    {
        private DBHandler Handler;

        private Dictionary<string, Account> Accounts;

        public EditTransferCommand(DBHandler handler, Dictionary<string, Account> accounts)
        {
            this.Handler = handler;
            this.Accounts = accounts;
        }
        public void Edit(Transfer transfer, string newName = null, string newIdentifier = null)
        {
            if (newName == null && newIdentifier == null)
            {
                throw new ArgumentNullException("Identifier or Name must be added to edit a Account.");
            }

            Dictionary<string, string> accAndTransferKeys = GetTransferAndAccountKeys(transfer);


            Fields field = new Fields();
            if (newName != null)
            {
                field.TransferName = newName;
                transfer.Name = newName;
            }

            if (newIdentifier != null)
            {
                field.TransferIdentifier = newIdentifier;
                transfer.Identifier = newIdentifier;
            }

            this.Handler.EditByID(transfer.ID, field);

            this.Accounts[accAndTransferKeys["accountKey"]].Transfers.Remove(accAndTransferKeys["transferKey"]);
            this.Accounts[accAndTransferKeys["accountKey"]].Transfers.Add(transfer.Name, transfer);

        }

        private Dictionary<string, string> GetTransferAndAccountKeys(Transfer transfer)
        {
            Dictionary<string, string> accAndTransferKeys = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Account> acc in this.Accounts)
            {
                foreach(KeyValuePair<string, Transfer>tr in acc.Value.Transfers)
                {
                    if(tr.Value == transfer)
                    {
                        accAndTransferKeys.Add("accountKey", acc.Key);
                        accAndTransferKeys.Add("transferKey", tr.Key);
                        return accAndTransferKeys;
                    }
                }
            }
            throw new Exception("This transfer does not exist.");
        }
    }
}
