using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TaskManagement.Domain.Entities.Identity;
using TaskManagement.Shared.Enums;

namespace TaskManagement.Domain.Entities
{
    public class AppTask:IAuditEntity,IDeletedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public AppTaskStatus Status { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeletionUser { get; set; }
        public string? CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
