using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskManagement.BusinessLogic.DTOs
{
    public class AppTaskCreateUpdateModel
    {
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        [Range(1,3)]
        public TaskStatus Status { get; set; }
        [Range(1,10)]
        public int Priority { get; set; }

    }
}
