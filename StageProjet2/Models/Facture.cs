using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageProjet2.Models
{
    public class Facture
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Etat { get; set; }
        [Required]
        public  double AncienValeur  { get; set; }
        [Required]
        public  double NouvelleValeur  { get; set; }
        public DateTime? FactureDate { get; set; }
        public double prix { get; set; }
        public Adherent Adherent { get; set; }
        public int? AdherentId { get; set; }
        public Compteur Compteur { get; set; }
        public int? CompteurId { get; set; }


    }
}
