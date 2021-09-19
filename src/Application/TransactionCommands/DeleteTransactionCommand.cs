using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;
using System.Reflection;

namespace Application.TransactionCommands
{
    public class DeleteTransactionCommand
    {
        public Account TransactionsAccount;
        public Transfer TransactionsTransfer;
        
        private DBHandler Handler;

        public DeleteTransactionCommand(DBHandler handler, Account account, Transfer transfer)
        {
            this.Handler = handler;
            this.TransactionsAccount = account;
            this.TransactionsTransfer = transfer;
        }

        public void Delete(Transaction transaction)
        {
            Fields field = new Fields()
            {
                TransactionID = transaction.ID
            };

            this.Handler.DeleteBy(field);
            this.TransactionsTransfer.Transactions.RemoveAll(x => x.ID == transaction.ID);
            this.TransactionsAccount.Transactions.RemoveAll(x => x.ID == transaction.ID);
        }
    }
}
