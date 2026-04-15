using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Shared.Exceptions
{
    public class GenericException : AppException
    {
        public GenericException(string errors, int statusCode)
            : base(errors, statusCode)
        {
        }
    }
}
