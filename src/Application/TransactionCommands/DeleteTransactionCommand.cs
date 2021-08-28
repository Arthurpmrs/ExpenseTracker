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
        private DBHandler Handler;

        public DeleteTransactionCommand(DBHandler handler)
        {
            this.Handler = handler;
        }

        public void Delete(Transaction transaction)
        {
            Fields field = new Fields()
            {
                TransactionID = transaction.ID
            };

            this.Handler.DeleteBy(field);
        }
    }
}
