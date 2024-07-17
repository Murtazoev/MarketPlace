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
            {
                newProduct = new Product();
            }
        }
        public IActionResult createProduct()
        {
            newProduct.Name = "Salom Aleykum";
            newProduct.ContactNumebr = "+99298asd123123";
            newProduct.Info = "an information about the product";
            return View(newProduct);
        }
        // [HttpPost]
        public IActionResult Increase()
        {
            newProduct.Id++;
            return RedirectToAction("createProduct");
        }
    }
}
