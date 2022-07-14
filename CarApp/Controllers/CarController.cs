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

        private Task<int> GenerateRandomId() {
            var cars = carService.Get().Result;
            var randomCarIndex = 0;
            var randomId = 0;

            if (cars != null && cars.Count > 0) {
                Random randomNumber = new();
                randomCarIndex = randomNumber.Next(0, cars.Count);
                randomId = cars.ElementAt(randomCarIndex).Id;
            }

            return Task.Run(() => randomId);
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

        public async Task<IActionResult> GuessPrice() //int? id
        {
            numberOfGuessAttempts = 0;

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var car = await carService.Find(id.Value);
            var randomId = await GenerateRandomId();
            var car = await carService.Find(randomId);

            //if (car == null)
            //{
            //    return NotFound();
            //}

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

            if (numberOfGuessAttempts > maxGuessAttempts)
            {
                ViewBag.messageMaxGuestAttempts = "Unfortunately you reached the maximum attempts. Max Guess Attempts = " + maxGuessAttempts;
            }

            numberOfGuessAttempts++;

            return View(car);
        }
    }
}