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
        public IActionResult Profile()
        {
            return View(new NewProduct());
        }
        [HttpPost]
        public IActionResult Profile(NewProduct newProduct )
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
            _dataBase.AddProduct(product);
            return RedirectToAction("Index" , "Home");
        }
        // [Route("/ClientController/removeProduct/{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _dataBase.SearchProduct(id);
            _dataBase.DeleteProduct(product);
            return RedirectToAction("Index" , "Home");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = _dataBase.SearchProduct(id);
            if (product == null)
                return RedirectToAction("Index" , "Home");
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (_dataBase.SearchProduct(id) == null)
                return RedirectToAction("Index" , "Home");
            Console.WriteLine(product.Name);
            _dataBase.Update(id, product);
            return RedirectToAction("Index" , "Home");
        }
    }
}
