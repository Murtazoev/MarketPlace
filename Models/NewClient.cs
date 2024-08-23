namespace WebApplication2.Models
{
    public class NewClient
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string ContactNumber { get; set; }
        public IFormFile? Avatar { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
