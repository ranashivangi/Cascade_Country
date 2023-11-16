using Cascade_Country.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cascade_Country.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;
        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult List()
        {
            List<Customer> customers = _context.Customers.Include(c => c.Country).Include(s => s.State).ToList();
            return View(customers);
        }
        private List<SelectListItem> GetCountries()
        {
            var lstCountries = new List<SelectListItem>();
            List<Country> Countries = _context.Countries.ToList();
            lstCountries = Countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.CountryName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Country----"
            };
            lstCountries.Insert(0, defItem);
            return lstCountries;
        }

        private List<SelectListItem> GetStates(int CountryId = 1)
        {
            List<SelectListItem> lstStates = _context.States.Where(c => c.CountryId == CountryId).OrderBy(n => n.StateName).Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.StateName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select State----"
            };
            lstStates.Insert(0, defItem);
            return lstStates;
        }
        [HttpGet]
        public IActionResult Create()
        {
            Customer Customer = new Customer();
            ViewBag.CountryId = GetCountries();
            ViewBag.StateId = GetStates();
            return View(Customer);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            _context.Add(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }
        [HttpGet]
        public JsonResult GetStatesByCountry(int CountryId)
        {
            List<SelectListItem> states = GetStates(CountryId);
            return Json(states);
        }

        private Customer GetCustomer(int Id)
        {
            Customer customer = _context.Customers
                .Where(c => c.Id == Id).FirstOrDefault();
            return customer;
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Customer customer = GetCustomer(Id);
            ViewBag.CountryName = GetCountryName(customer.CountryId);
            ViewBag.StateName = GetStateName(customer.StateId);
            return View(customer);
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Customer customer = GetCustomer(Id);
            ViewBag.CountryName = GetCountryName(customer.CountryId);
            ViewBag.StateName = GetStateName(customer.StateId);
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }




        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Customer customer = GetCustomer(Id);
            ViewBag.CountryId = GetCountries();
            ViewBag.StateId = GetStates(customer.CountryId);
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }

        private string GetCountryName(int CountryId)
        {
            string countryName = _context.Countries.Where(ct => ct.Id == CountryId).SingleOrDefault().CountryName;
            return countryName;
        }

        private string GetStateName(int StateId)
        {
            string cityName = _context.States.Where(ct => ct.Id == StateId).SingleOrDefault().StateName;
            return cityName;
        }
    }
}
