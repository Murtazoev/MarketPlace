using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly DataBase _dataBase;
        public HomeController(ILogger<HomeController> logger, DataBase dataBase)
        {
            _logger = logger;
            _dataBase = dataBase;
        }

        public IActionResult Index()
        {
            var ListOfProducts = _dataBase.GetListProducts();
            return View(ListOfProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
