using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskManagement.BusinessLogic.DTOs
{
    public class AppTaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskStatus Status { get; set; }
        public int Priority { get; set; }
    }
}
