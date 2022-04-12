using System;
using System.Collections.Generic;

namespace InstantAPI.Models
{
    public partial class Article
    {
        public Article()
        {
            IdImages = new HashSet<Image>();
            IdTags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public string? Titre { get; set; }
        public string? Texte { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? IdAuteur { get; set; }

        public virtual Auteur? IdAuteurNavigation { get; set; }

        public virtual ICollection<Image> IdImages { get; set; }
        public virtual ICollection<Tag> IdTags { get; set; }
    }
}
