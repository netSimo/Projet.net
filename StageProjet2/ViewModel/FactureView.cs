using System.ComponentModel.DataAnnotations;

namespace StageProjet2.ViewModel
{
    public class FactureView
    {

        public int Id { get; set; }
        public int IdAdherent { get; set; }
        public int IdCompteur { get; set; }
        public string Etat { get; set; }

        [Required] 
        public string Libelle { get; set; }
       
        public double AncienValeur { get; set; }
       
        public double NouvelleValeur { get; set; }

        public DateTime? FactureDate { get; set; }

        public double prix { get; set; }
    }
}
