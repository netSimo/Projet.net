using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageProjet2.Models
{
    public class Prix
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        public  double Prixtranche1 { get; set; }

        [Required]
        public double Prixtranche2 { get; set; }

        [Required]
        public double Prixtranche3 { get; set; }

        public DateTime? DateTimePrix { get; set; }

        public string etat { get; set; }
    }
}
