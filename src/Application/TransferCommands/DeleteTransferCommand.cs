using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure;

namespace Application.TransferCommands
{
    public class DeleteTransferCommand
    {
        public Account TransfersAccount;
        private DBHandler Handler;

        public DeleteTransferCommand(DBHandler handler, Account account)
        {
            this.Handler = handler;
            this.TransfersAccount = account;
        }
        public void Delete(Transfer transfer)
        {
            Fields field = new Fields()
            {
                TransferID = transfer.ID
            };

            this.Handler.DeleteBy(field);
            foreach (KeyValuePair<string, Transfer> tr in this.TransfersAccount.Transfers)
            {
                if (tr.Value.ID == transfer.ID) {
                    this.TransfersAccount.Transfers.Remove(tr.Key);
                }
            }
        }
    }
}
