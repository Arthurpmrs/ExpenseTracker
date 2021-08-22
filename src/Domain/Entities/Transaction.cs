using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction
    {
        public long ID { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }
        public string Tag { get; set; }
        public string Date { get; set; }
        public string DateAdded { get; set; }
        public long TransferID { get; set; }
        public long AccountID { get; set; }
    }
}
