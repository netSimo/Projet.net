using Microsoft.AspNetCore.Identity;

namespace StageProjet2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string  Nom { get; set; }
        public string  Prenom { get; set; }
    }
}
