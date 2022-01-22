using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.TransactionCommands
{
    public class LoadTransactionsCommand
    {
        private DBHandler Handler;
        public Account TransactionsAccount { get; set; }
        public Transfer TransactionsTransfer { get; set; }
        

        public LoadTransactionsCommand(DBHandler handler, Account account = null, Transfer transfer = null)
        {
            this.Handler = handler;
            this.TransactionsAccount = account;
            this.TransactionsTransfer = transfer;
        }

        public List<Transaction> LoadAccountTransactions()
        {
            List<Transaction> Transactions = new List<Transaction>();
            
            List<Fields> transactionsFields = this.Handler.GetAll(new Fields { AccountID = this.TransactionsAccount.ID });

            if (transactionsFields == null) 
            {
                throw new EmptyStorageException("There are no Transactions in this account");
            }
            else
            {
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
                this.TransactionsAccount.Transactions = Transactions;
                return Transactions;
            }  
        }

        public List<Transaction> LoadTransferTransactions()
        {
            List<Transaction> Transactions = new List<Transaction>();
            List<Fields> transactionsFields = this.Handler.GetAll(new Fields { TransferID = this.TransactionsTransfer.ID });
            if (transactionsFields == null)
            {
                throw new EmptyStorageException("There are no Transactions in this transfer.");
            }
            else
            {
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
                this.TransactionsTransfer.Transactions = Transactions;
                return Transactions;
            }
        }
        
    }
}
