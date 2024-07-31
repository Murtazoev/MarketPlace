using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        static Product newProduct;

        int cnt = 0;
        public ProductController()
        {
            if (newProduct  == null)            
                newProduct = new Product();               
        }
    }
}
