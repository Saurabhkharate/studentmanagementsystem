using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using studentmanagementsystem.DatabaseContext;
using studentmanagementsystem.Interface;
using studentmanagementsystem.Models;
using studentmanagementsystem.Repositories;
using System.Threading.Tasks;

namespace studentmanagementsystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository ;
        private readonly IWebHostEnvironment webHostEnvironment ;
        private readonly AppDatabaseContext appDatabaseContext ;
        public StudentController(IStudentRepository _studentRepository, IWebHostEnvironment webHostEnvironment, AppDatabaseContext _appDatabaseContext)
        {
            this.studentRepository = _studentRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.appDatabaseContext = _appDatabaseContext;
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
                model.Age = DateTime.Now.Year - model.DOB.Year;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    model.ImagePath = "/Uploads/" + fileName;
                }

                model.Status = true;

                await studentRepository.SaveStudent(model);
                await studentRepository.Save();

                return RedirectToAction("Index");
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
        private void LoadDropdowns()
        {
            
                ViewBag.Country = new SelectList(appDatabaseContext.Countries.ToList(), "Id", "CountryName");
            
        }
        // load states based on country
        public IActionResult GetStates(int countryId)
        {
            var states = appDatabaseContext.States
                .Where(x => x.CountryId == countryId)
                .ToList();

            return PartialView("_StateDropdown", states);
        }

        // load cities based on state
        public IActionResult GetCities(int stateId)
        {
            var cities = appDatabaseContext.Cities
                .Where(x => x.StateId == stateId)
                .ToList();

            return PartialView("_CityDropdown", cities);
        }


    }
}