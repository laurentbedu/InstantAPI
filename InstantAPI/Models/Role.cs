using System;
using System.Collections.Generic;

namespace InstantAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
