using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string images { get; set; }
        public string Name { get; set; }
        public string Info {  get; set; }
        public decimal Price { get; set; }
    }
}