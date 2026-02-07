using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using studentmanagementsystem.Interface;
using studentmanagementsystem.Models;

public class MasterController : Controller
{
    private readonly IMasterRepository masterRepo;

    public MasterController(IMasterRepository _masterRepo)
    {
        masterRepo = _masterRepo;
    }

    // COUNTRY
    public async Task<IActionResult> CountryList()
    {
        var data = await masterRepo.GetCountries();
        return View(data);
    }

    public IActionResult AddCountry()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddCountry(Country model)
    {
        await masterRepo.AddCountry(model);
        await masterRepo.Save();
        return RedirectToAction("CountryList");
    }

    // STATE
    public async Task<IActionResult> StateList()
    {
        var data = await masterRepo.GetStates();
        return View(data);
    }

    public async Task<IActionResult> AddState()
    {
        ViewBag.Country = new SelectList(await masterRepo.GetCountries(), "Id", "CountryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddState(State model)
    {
        await masterRepo.AddState(model);
        await masterRepo.Save();
        return RedirectToAction("StateList");
    }

    // CITY
    public async Task<IActionResult> CityList()
    {
        var data = await masterRepo.GetCities();
        return View(data);
    }

    public async Task<IActionResult> AddCity()
    {
        ViewBag.Country = new SelectList(await masterRepo.GetCountries(), "Id", "CountryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddCity(City model)
    {
        await masterRepo.AddCity(model);
        await masterRepo.Save();
        return RedirectToAction("CityList");
    }

    public async Task<JsonResult> GetStates(int countryId)
    {
        var states = await masterRepo.GetStatesByCountry(countryId);
        return Json(states);
    }
}