using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StageProjet2.Models
{
    public class Compteur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Libelle { get; set; }

        public string Marque { get; set; }

        public Adherent Adherent { get; set; }  
    }
}
