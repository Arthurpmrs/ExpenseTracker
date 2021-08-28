using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Fields
    {
        public long AccountID { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public long TransferID { get; set; }
        public string TransferType { get; set; }
        public string TransferName { get; set; }
        public string TransferIdentifier { get; set; }
        public long TransactionID { get; set; }
        public double TransactionValue { get; set; }
        public string TransactionTag { get; set; }
        public string TransactionNote { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionDateAdded { get; set; }
    }
}
