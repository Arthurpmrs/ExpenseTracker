using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Entities.TransferTypes
{
    class Transference : Transfer
    {
        private string _type;
        private long  _id;
        private long _accountID;
        private string _name;
        private string _identifier;

        public Transference(long id, long accountID, string name, string identifier)
        {
            this._id = id;
            this._accountID = accountID;
            this._name = name;
            this._type = "Transference";
            this._identifier = identifier;
        }
        public override string Type
        {
            get { return _type; }
        }
        public override long ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public override long AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public override string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
    }
}
