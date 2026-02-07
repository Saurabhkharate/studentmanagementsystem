using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using studentmanagementsystem.DatabaseContext;
using studentmanagementsystem.Models;

namespace studentmanagementsystem.Controllers
{
    public class MasterController : Controller
    {
        private readonly AppDatabaseContext db;

        public MasterController(AppDatabaseContext context)
        {
            db = context;
        }

        // ================= COUNTRY =================

        public IActionResult CountryList()
        {
            var data = db.Countries.ToList();
            return View(data);
        }

        public IActionResult AddCountry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCountry(Country model)
        {
            db.Countries.Add(model);
            db.SaveChanges();
            return RedirectToAction("CountryList");
        }

        // ================= STATE =================

        public IActionResult StateList()
        {
            var data = db.States.Include(x => x.Country).ToList();
            return View(data);
        }

        public IActionResult AddState()
        {
            ViewBag.Country = new SelectList(db.Countries, "Id", "CountryName");
            return View();
        }

        [HttpPost]
        public IActionResult AddState(State model)
        {
            db.States.Add(model);
            db.SaveChanges();
            return RedirectToAction("StateList");
        }

        // ================= CITY =================

        public IActionResult CityList()
        {
            var data = db.Cities.Include(x => x.State).ThenInclude(s => s.Country).ToList();
            return View(data);
        }

        public IActionResult AddCity()
        {
            ViewBag.Country = new SelectList(db.Countries, "Id", "CountryName");
            return View();
        }

        [HttpPost]
        public IActionResult AddCity(City model)
        {
            db.Cities.Add(model);
            db.SaveChanges();
            return RedirectToAction("CityList");
        }

        // AJAX for state dropdown
        public JsonResult GetStates(int countryId)
        {
            var states = db.States.Where(x => x.CountryId == countryId).ToList();
            return Json(states);
        }
    }
}
