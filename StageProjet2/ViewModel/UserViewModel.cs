namespace  StageProjet2.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public  IList<string> Role { get; set; }
    }
}
