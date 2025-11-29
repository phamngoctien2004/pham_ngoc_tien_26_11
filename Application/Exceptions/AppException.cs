using Core.Enums;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public ErrorStatus ErrorStatus { get; }
        
        public AppException(ErrorStatus errorStatus) : base(errorStatus.ToString()) { 
            ErrorStatus = errorStatus;
        }
    }
}
