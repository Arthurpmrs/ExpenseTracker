using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure;


namespace Application.TransferCommands
{
    public class LoadTransfersCommand
    {
        private DBHandler Handler;
        private Account TransfersAccount;

        public LoadTransfersCommand(DBHandler handler, Account account)
        {
            this.Handler = handler;
            this.TransfersAccount = account;
        }

        public Dictionary<string, Transfer> Load()
        {
            Dictionary<string, Transfer> Transfers = new Dictionary<string, Transfer>();
            List<Fields> transfersFields = this.Handler.GetAll(new Fields { AccountID = this.TransfersAccount.ID});
            if (transfersFields != null)
            {
                foreach (Fields f in transfersFields)
                {
                    Enum.TryParse(f.TransferType, out TransferType type);
                    string transferName = f.TransferName;
                    Transfer transferObject = TransferFactory.Create(
                        type,
                        f.TransferID,
                        f.AccountID,
                        f.TransferName,
                        f.TransferIdentifier
                        );
                    Transfers.Add(transferName, transferObject);
                    this.TransfersAccount.AddTransfer(transferObject);
                }
            } else
            {
                throw new EmptyStorageException($"There is no Transfer in >{this.TransfersAccount.Name}< account.");
            }
            return Transfers;
        }
        public Transfer LoadTransferByName(string name)
        {
            Fields fields = this.Handler.GetByName(name);
            if (fields == null)
            {
                throw new Exception("There is no such transfer in DataBase.");
            }
            else
            {
                Enum.TryParse(fields.TransferType, out TransferType type);
                Transfer transfer = TransferFactory.Create(
                    type,
                    fields.TransferID,
                    fields.AccountID,
                    fields.TransferName,
                    fields.TransferIdentifier
                    );
                return transfer;
            }
        }
    }
}
