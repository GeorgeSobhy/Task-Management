using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Shared.Exceptions
{
    public class UserAlreadyExistsException : AppException
    {
        public UserAlreadyExistsException(string email)
            : base($"User with email '{email}' already exists.", 409)
        {
        }
    }
}
