using Microsoft.AspNetCore.Http;


namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository : IGenericeRepository<Entities.AppTask>
    {
    }

}
