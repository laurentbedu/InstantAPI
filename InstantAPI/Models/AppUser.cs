namespace InstantAPI.Models
{
    public partial class AppUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public int IdRole { get; set; }
        public int IdAuteur { get; set; }

        public virtual Auteur IdAuteurNavigation { get; set; } = null!;
        public virtual Role IdRoleNavigation { get; set; } = null!;
    }
}
