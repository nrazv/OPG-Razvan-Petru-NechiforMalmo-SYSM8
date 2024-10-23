using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.AppExceptions;

public class UserTaken : Exception
{
    public UserTaken() : base("Username is taken") { }
}
