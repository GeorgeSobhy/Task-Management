using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public interface IDeletedEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionDate { get; set; }
        string? DeletionUser { get; set; }
    }
}
