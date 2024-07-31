namespace WebApplication2.Models
{
    public class NewProduct
    {
        public IFormFile? images { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public int Price { get; set; }
    }
}