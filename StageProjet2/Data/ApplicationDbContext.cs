using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StageProjet2.Models;
using StageProjet2.ViewModel;

namespace StageProjet2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Adherent> adherents { get; set; }

        public DbSet<Compteur> compteurs { get; set; }

        public DbSet<Facture> factures { get; set; }

        public DbSet<Prix> prixs { get; set; }

        public DbSet<StageProjet2.ViewModel.PrixView> PrixView { get; set; } = default!;

        //public DbSet<StageProjet2.ViewModel.FactureView> FactureView { get; set; } = default!;
    }
}