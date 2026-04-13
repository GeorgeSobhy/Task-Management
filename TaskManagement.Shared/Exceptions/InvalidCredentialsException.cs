using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Shared.Exceptions
{
    public class InvalidCredentialsException : AppException
    {
        public InvalidCredentialsException()
            : base("Invalid email or password.", 401)
        {
        }
    }
}
