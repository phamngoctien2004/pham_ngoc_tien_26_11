using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum ErrorStatus
    {
        UnAuthentication = 401,
        UnAuthorization = 403,

        EmailExisted = 1000,

        //file 
        FileNotEmpty = 2000,
        FileTooLarge = 2001,
        FileNotAllowed = 2002
    }
}
