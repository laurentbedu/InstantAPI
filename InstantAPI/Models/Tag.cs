using System;
using System.Collections.Generic;

namespace InstantAPI.Models
{
    public partial class Tag
    {
        public Tag()
        {
            IdArticles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Mot { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
