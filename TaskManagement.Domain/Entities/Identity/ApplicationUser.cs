using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser,IAuditEntity,IDeletedEntity
    {
        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeletionUser { get; set; }
        public string? CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
