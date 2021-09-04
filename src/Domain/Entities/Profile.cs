using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Profile
    {
        public string Name { get; set; }
        public Dictionary<string, Account> Accounts { get; private set; } = new Dictionary<string, Account>();

        public Profile(string name)
        {
            this.Name = name;
        }
    }
}
