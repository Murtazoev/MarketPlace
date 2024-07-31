using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ClientController : Controller
    {
        static Client newClient;
        static ProductController productController;

        DataBase dataBase = new DataBase();

        public ClientController ()
        {
            if (newClient == null)
            {
                newClient = new Client();
                ProductController productController = new ProductController();
                newClient.products = new List<Product>();
            }
        }
        [HttpGet]
        public IActionResult Profile()
        {
            return View(new NewProduct());
        }
        [HttpPost]
        public IActionResult Profile(NewProduct newProduct )
        {
            if (newProduct.images == null)
            {
                ModelState.AddModelError("images" , "The image is required");
                return View();
            }
            if (ModelState.IsValid != true)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }
            string fileName;
            fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            fileName += Path.GetExtension(newProduct.images.FileName);
            string ImageFullPath = Directory.GetCurrentDirectory() + "/wwwroot/image/" + fileName;
            using (var stream = System.IO.File.Create(ImageFullPath))
            {
                newProduct.images.CopyTo(stream);
            }
            Product product = new Product()
            {
                Name = newProduct.Name,
                Info = newProduct.Info,
                images = fileName,
                Price = 0,
            };
            if (newClient.products.Count == 0)
                product.Id = 0;
            else
                product.Id = newClient.products.Last().Id - 1;
            newClient.products.Add(product);
            return RedirectToAction("List_Clients");
        }
        public IActionResult List_Clients()
        {
            var listOfProducts = dataBase.GetListProducts();
            return View(listOfProducts);
        }
        // [Route("/ClientController/removeProduct/{id}")]
        public IActionResult Delete(int id)
        {
            Product product = newClient.products.Find(x => x.Id == id);
            if (product == null)
                return RedirectToAction("List_Clients");
            newClient.products.Remove(product);
            return RedirectToAction("List_Clients");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = newClient.products.Find(x => x.Id == id);
            if (product == null)
                return RedirectToAction("List_Clients");
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            Product newProduct = newClient.products.Find(x => x.Id == id) ;
            if (newProduct == null)
                return RedirectToAction("List_Clients");
            newClient.products.Find(x => x.Id == id).Name = product.Name;
            newClient.products.Find(x => x.Id == id).Info = product.Info;
            return RedirectToAction("List_Clients");
        }
    }
}
