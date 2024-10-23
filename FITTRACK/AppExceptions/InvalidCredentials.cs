using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.AppExceptions
{
    internal class InvalidCredentials : Exception
    {
        public InvalidCredentials() : base("Invalid credentials") { }
    }
}
