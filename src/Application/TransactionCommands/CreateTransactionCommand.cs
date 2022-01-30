using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Domain.Entities;

namespace Application.TransactionCommands
{
    public class CreateTransactionCommand
    {
        private DBHandler Handler;
        private Account Account;
        private Transfer Transfer;

        public CreateTransactionCommand(DBHandler handler, Account account, Transfer transfer)
        {
            this.Handler = handler;
            this.Account = account;
            this.Transfer = transfer;
        }
        public Transaction Create(double value, string tag = "", string note = "", string date = "")
        {
            string dateAdded = DateTime.Now.ToString("yyyy-MM-dd");
            if (date == "")
            {
                date = dateAdded;
            }
            Transaction transaction;
            Fields fields = new Fields()
            {
                TransactionValue = value,
                TransactionTag = tag,
                TransactionNote = note,
                TransactionDate = date,
                TransactionDateAdded = dateAdded,
                TransferID = Transfer.ID,
                AccountID = Account.ID
            };
            long ID = this.Handler.Insert(fields);
            transaction = new Transaction(
                ID,
                value,
                note,
                tag,
                date,
                dateAdded,
                Transfer.ID,
                Account.ID
                );
            return transaction;
        }

    }

}
