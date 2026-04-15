using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public interface IAuditEntity
    {
        string? CreationUser { get; set; }
        DateTime CreationDate { get; set; }
        string? ModificationUser { get; set; }
        DateTime? ModificationDate { get; set; }
    }
}
