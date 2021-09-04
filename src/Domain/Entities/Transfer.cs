using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.TransferTypes;

namespace Domain.Entities
{
    public abstract class Transfer
    {
        public abstract string Type { get; }
        public abstract long ID { get; set; }
        public abstract long AccountID { get; set; }
        public abstract string Name { get; set; }
        public abstract string Identifier { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public enum TransferType
    {
        Transference,
        Pix
    }

    public static class TransferFactory
    {
        public static Transfer Create(TransferType type, long id, long accountID, string name = "", string identifier = "")
        {
            switch (type)
            {
                default:
                    throw (new KeyNotFoundException($"No such Transfer as >{type}<"));
                case TransferType.Transference:
                    return new Transference(id, accountID, name, identifier);
                case TransferType.Pix:
                    return new Pix(id, accountID, name, identifier);

            }
        }
    }
}
