using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.BusinessLogic.DTOs.Responses
{
    public abstract class BaseResponse
    {
        public string Message { get; set; } = null!;
        public bool Success { get; set; }
        public string? Error { get; set; }
        public int StatusCode { get; set; }
    }
}
