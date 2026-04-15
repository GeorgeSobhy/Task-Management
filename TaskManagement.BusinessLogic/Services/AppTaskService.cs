using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.BusinessLogic.Interfaces;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.BusinessLogic.Services
{
    public class AppTaskService : IAppTaskService
    {
        private readonly ITaskRepository _appTaskRepository;
        public AppTaskService(ITaskRepository appTaskRepository)
        {
            _appTaskRepository = appTaskRepository;
        }
        public async Task<bool> IsDuplicateAsync(string title, string userId, DateTime time)
        {
            return await _appTaskRepository.GetMany().AnyAsync(t => t.Title == title && t.UserId == userId && t.CreatedAt.Date == time.Date);
        }
    }
}
