using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Entities.AppTask>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    : base(context, httpContextAccessor)
        {

        }
    }
}
