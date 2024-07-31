namespace WebApplication2.Models
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Contact_Number { get; set; }
        public List<Product> products { get; set; }
    }
}
