using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.BusinessLogic.DTOs
{
    public class ApplicationUserModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public ICollection<AppTaskModel> Tasks { get; set; } = new List<AppTaskModel>();

    }
}
