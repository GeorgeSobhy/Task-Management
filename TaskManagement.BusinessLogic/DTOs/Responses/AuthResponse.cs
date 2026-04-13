using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.BusinessLogic.DTOs.Responses
{
    public class AuthResponse : BaseResponse
    {
        public string Token { get; set; } = null!;
    }
}
