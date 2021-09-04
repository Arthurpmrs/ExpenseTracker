using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
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

        public List<Transfer> Load()
        {
            List<Transfer> Transfers = new List<Transfer>();
            List<Fields> transfersFields = this.Handler.GetAll(new Fields { AccountID = this.TransfersAccount.ID});

            foreach (Fields f in transfersFields)
            {
                Enum.TryParse(f.TransferType, out TransferType type);
                Transfers.Add(
                    TransferFactory.Create(
                        type,
                        f.TransferID,
                        f.AccountID,
                        f.TransferName,
                        f.TransferIdentifier
                        )
                    );
            }

            return Transfers;
        }
    }
}
