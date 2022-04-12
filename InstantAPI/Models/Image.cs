using System;
using System.Collections.Generic;

namespace InstantAPI.Models
{
    public partial class Image
    {
        public Image()
        {
            IdArticles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Filepath { get; set; } = null!;
        public string? Titre { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
