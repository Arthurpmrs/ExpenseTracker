using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class EmptyStorageException : Exception
    {
        public EmptyStorageException() { }

        public EmptyStorageException(string message) : base(message)
        {

        }
        public EmptyStorageException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
