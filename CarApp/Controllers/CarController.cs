using Microsoft.AspNetCore.Mvc;
using CarApp.Services.Specification;
using CarApp.Models;

namespace CarApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        // GET: CarController
        public async Task<IActionResult> Index()
        {
            var cars = await carService.Get();
            return View(cars);
        }

        public async Task<IActionResult> AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Create" : "Edit";
            ViewBag.IsEdit = id == null ? false : true;

            if (id == null)
            {
                return View();
            }
            else
            {
                var car = await carService.Find(id.Value);

                if (car == null)
                {
                    return NotFound();
                }

                return View(car);
            }
        }

        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,Make,Model,Year,Doors,Color,Price")] Car carData)
        {
            if (ModelState.IsValid)
            {
                carService.AddOrEdit(carData);

                return RedirectToAction(nameof(Index));
            }

            return View(carData);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await carService.Find(id.Value);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await carService.Find(id.Value);

            return View(car);
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            carService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GuessPrice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await carService.Find(id.Value);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuessPrice(int id)
        {
            var car = await carService.Find(id);

            if (car == null)
            {
                return NotFound();
            }

            var guessedPrice = Request.Form["price"];

            if (car.Price == guessedPrice) {
                ViewBag.message = "Great Job!";
            }

            return View(car);
        }
    }
}