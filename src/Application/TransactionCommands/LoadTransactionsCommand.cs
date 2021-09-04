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
        public Account TransactionsAccount { get; set; }
        public Transfer TransactionsTransfer { get; set; }
        

        public LoadTransactionsCommand(DBHandler handler, Account account, Transfer transfer)
        {
            this.Handler = handler;
            this.TransactionsAccount = account;
            this.TransactionsTransfer = transfer;
        }

        public List<Transaction> LoadAccountTransactions()
        {
            List<Transaction> Transactions = new List<Transaction>();
            
            List<Fields> transactionsFields = this.Handler.GetAll(new Fields { AccountID = this.TransactionsAccount.ID });
            
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
