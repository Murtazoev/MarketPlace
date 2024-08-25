using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;
using WebApplication2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;

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
        [HttpPost]
        public IActionResult Index(Product product)
        {
            Console.WriteLine("Ma da hami darunm"); 
            // Buy(product);
            return RedirectToAction("Buy" , "Home" , product);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(NewClient newClient)
        {
            bool ok = false;
            if (newClient.Password != newClient.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "The passwords do not match");
                ModelState.AddModelError("ConfirmPassword", "The passwords do not match");
                ok = true;
            }
            if (newClient.Avatar == null)
            {
                ModelState.AddModelError("Avatar", "The image is required !!!");
                ok = true;
            }
            if (newClient.Mail == null)
            {
                ModelState.AddModelError("Email", "The mail is required !!!");
                ok = true;
            }
            if (newClient.ContactNumber == null)
            {
                ModelState.AddModelError("Contact", "The Contact number is required !!!");
                ok = true;
            }
            if (ok == true)
                return View();

            Client client = new Client();
            int newId = _dataBase.NumberOfClients();
            client.Id = newId;
            client.Name = newClient.Name;
            client.Surname = newClient.Surname;
            client.Contact_Number = newClient.ContactNumber;
            client.Email = newClient.Mail;
            client.Password = newClient.Password;

            string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            FileName += Path.GetExtension(newClient.Avatar.FileName);
            string FullPath = Directory.GetCurrentDirectory() + "/wwwroot/image/" + FileName;

            /// Sending avatar to the specific folder
            using (var stream = System.IO.File.Create(FullPath))
                newClient.Avatar.CopyTo(stream);

            client.AvatarLocation = FileName;
            _dataBase.AddClient(client);
            Console.WriteLine(FullPath);


            return RedirectToAction("ListOfClients" , "Home");
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

        public IActionResult ListOfClients()
        {
            return View(_dataBase.GetClients());
        }
        [HttpGet]
        public IActionResult Login()
        {
            /*
            ClaimsPrincipal claim = HttpContext.User;
            if (claim.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");*/
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid || model.Username == null)
            {
                Console.WriteLine("Something is wrong");
                ModelState.AddModelError("Username" ,"Something went wrong");
                return View();
            }
            Client searchClient = _dataBase.SearchClient(model.Username);
            if (searchClient == null)
            {
                Console.WriteLine("searchClient null");
                ModelState.AddModelError("Username", "Username not found !!!");
                return View();
            }
            if (searchClient.Password != model.Password)
            {
                Console.WriteLine("Wrong Password");
                Console.WriteLine(model.Password + " " + model.Password.Length);
                Console.WriteLine(searchClient.Password + " " + searchClient.Password.Length);
                ModelState.AddModelError("Password", "Maybe you forgot but your password is " + searchClient.Password);
                return View();
            }
            Console.WriteLine("Everything Works Fineee !");



            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims , CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                // IsPersistent = true
            };
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , new ClaimsPrincipal(claimsIdentity) , properties);

            return RedirectToAction("Privacy", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.WriteLine("User Logged Out");
            return RedirectToAction("Login");
        }
        public IActionResult MyPage()
        {
            Client client = _dataBase.SearchClient(User.Identity.Name);
            var list = _dataBase.GetClientsProduct(client.Id);
            return View(list);
        }
        public IActionResult Buy(Product prod)
        {
            return View(prod);
        }
    }
}
