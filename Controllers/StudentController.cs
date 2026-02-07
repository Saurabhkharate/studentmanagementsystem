using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using studentmanagementsystem.Helpers;
using studentmanagementsystem.Interface;
using studentmanagementsystem.Models;
namespace studentmanagementsystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository ;
        private readonly IWebHostEnvironment webHostEnvironment ;
        private readonly IMasterRepository masterRepository;
        public StudentController(IStudentRepository _studentRepository,
            IWebHostEnvironment webHostEnvironment,
            IMasterRepository masterRepository)
        {
            this.studentRepository = _studentRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.masterRepository = masterRepository;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            var students = await studentRepository.GetAll(page, pageSize);
            int total = await studentRepository.TotalCount();

            ViewBag.TotalPages = Math.Ceiling((double)total / pageSize);
            ViewBag.CurrentPage = page;

            return View(students);
        }

        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentInfo model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                model.Age = DateTime.Today.Year - model.DOB.Year;
                if (model.DOB.Date > DateTime.Today.AddYears(-model.Age))
                {
                    model.Age--;
                }
                try
                {
                    if (ImageFile != null)
                    {
                        model.ImagePath = await FileHelper.SaveImageAsync(ImageFile, webHostEnvironment.WebRootPath);
                    }

                    model.Status = true;

                    await studentRepository.SaveStudent(model);
                    await studentRepository.Save();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            LoadDropdowns();
            return View(model);
        }


        public async Task<ActionResult> Edit(int id)
        {
            var data = await studentRepository.GetById(id);
            LoadDropdowns();
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(StudentInfo model)
        {
            studentRepository.UpdateStudent(model);
            studentRepository.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, bool status)
        {
            await studentRepository.DeleteStudent(id, status);
            await studentRepository.Save();

            return Ok();
        }
        private async Task LoadDropdowns()
        {
            var countries = await masterRepository.GetCountries();
            ViewBag.Country = new SelectList(countries, "Id", "CountryName");
        }

        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await masterRepository.GetStatesByCountry(countryId);
            return PartialView("_StateDropdown", states);
        }

        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await masterRepository.GetCitiesByState(stateId);
            return PartialView("_CityDropdown", cities);
        }


    }
}