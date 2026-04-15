using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.BusinessLogic.Interfaces
{
    public interface IAppTaskService
    {
        Task<bool> IsDuplicateAsync(string title, string userId, DateTime time);
    }
}
