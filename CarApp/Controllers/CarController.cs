using Microsoft.AspNetCore.Mvc;
using CarApp.Services.Specification;
using CarApp.Models;

namespace CarApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService carService;
        private readonly int maxGuessAttempts;
        private readonly IConfiguration configuration;
        private static int numberOfGuessAttempts = 0;

        public CarController(ICarService carService, IConfiguration configuration)
        {
            this.carService = carService;
            this.configuration = configuration;

            maxGuessAttempts = Convert.ToInt32(configuration["MaxGuessAttempts"]);
        }

        private async Task<int> GenerateRandomId() {
            var cars = await carService.Get();
            var maxId = 1;

            if (cars != null && cars.Count > 0) {
                Random randomNumber = new();
                maxId = randomNumber.Next(cars.Min(car => car.Id), cars.Max(car => car.Id));
            }

            return maxId;
        }

        // GET: CarController
        public async Task<IActionResult> Index()
        {
            ViewBag.CarId = GenerateRandomId().Result;

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
            ViewBag.CarId = GenerateRandomId().Result;

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

            var guessedPrice = Request.Form["price"] == "" ? 0 : Convert.ToDecimal(Request.Form["price"]);

            if (car.Price == guessedPrice && numberOfGuessAttempts <= maxGuessAttempts) {
                ViewBag.message = "Great Job! The price is $" + guessedPrice;
                numberOfGuessAttempts = 0;
            }

            numberOfGuessAttempts++;

            return View(car);
        }
    }
}