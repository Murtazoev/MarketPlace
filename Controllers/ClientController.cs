using Microsoft.AspNetCore.Authorization;
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
        private int newId = 0;

        private readonly DataBase _dataBase;

        public ClientController (DataBase dataBase)
        {
            if (newClient == null)  
            {
                newClient = new Client();
                ProductController productController = new ProductController();
            }
            _dataBase = dataBase;
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(new NewProduct());
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddProduct(NewProduct newProduct)
        {
            if (newProduct.images == null)
            {
                ModelState.AddModelError("images" , "The image is required momange");
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
            /// Sending the Products data to DataBase
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
            product.Id = _dataBase.NextProductID();
            Client owner = new Client();
            owner = _dataBase.SearchClient(User.Identity.Name);
            product.Owner = owner.Id;
            _dataBase.AddProduct(product);
            return RedirectToAction("MyPage" , "Home");
        }
        // [Route("/ClientController/removeProduct/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Client client = new Client();
            client = _dataBase.SearchClient(User.Identity.Name);
            Product product = _dataBase.SearchProduct(id);
            if (client.Id == product.Owner)
                _dataBase.DeleteProduct(product);
            return RedirectToAction("MyPage" , "Home");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Console.WriteLine("Hello Boss");
            Product product = _dataBase.SearchProduct(id);
            Client client = new Client();
            client = _dataBase.SearchClient(User.Identity.Name);
            Console.WriteLine(User.Identity.Name);
            Console.WriteLine(client.Id + " " + product.Owner);
            Console.WriteLine(product.Name + " " + product.Id + " " + product.Owner);
            if (product.Owner != client.Id || product == null)
                return RedirectToAction("MyPage" , "Home");
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (_dataBase.SearchProduct(id) == null)
                return RedirectToAction("MyPage" , "Home");
            Console.WriteLine(product.Name);
            _dataBase.Update(id, product);
            return RedirectToAction("MyPage" , "Home");
        }
    }
}
