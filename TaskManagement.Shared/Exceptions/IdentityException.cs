using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Shared.Exceptions
{
    public class IdentityException : AppException
    {
        public IdentityException(string errors)
            : base(errors, 400)
        {
        }
    }
}
