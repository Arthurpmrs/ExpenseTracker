using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.TransactionCommands
{
    public class LoadTransactionsCommand
    {
        private DBHandler Handler;

        public LoadTransactionsCommand(DBHandler handler)
        {
            this.Handler = handler;
        }

        public List<Transaction> Load()
        {
            List<Transaction> Transactions = new List<Transaction>();

            List<Fields> transactionsFields = this.Handler.GetAll();
            foreach (Fields f in transactionsFields)
            {
                Transactions.Add(
                    new Transaction(
                        f.TransactionID,
                        f.TransactionValue,
                        f.TransactionNote,
                        f.TransactionTag,
                        f.TransactionDate,
                        f.TransactionDateAdded,
                        f.TransferID,
                        f.AccountID
                        )
                    );
            }
            return Transactions;
        }
        
    }
}
