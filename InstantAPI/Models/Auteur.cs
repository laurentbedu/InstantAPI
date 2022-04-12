using System;
using System.Collections.Generic;

namespace InstantAPI.Models
{
    public partial class Auteur
    {
        public Auteur()
        {
            AppUsers = new HashSet<AppUser>();
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string Pseudo { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
