using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StageProjet2.Models
{
    public class Adherent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        

        public   string Nom { get; set; }
        public   string Prenom { get; set; }
        public   string Adresse { get; set; }
        public int Num { get; set; }

        public int CompteurId { get; set; }
        public Compteur Compteur { get; set; }
    }
}
