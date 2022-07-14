using CarApp.Models;
using CarApp.Repository.Specification;

namespace CarApp.Repository
{
    public class CarRepository : ICarRepository
    {
        private static readonly List<Car> cars = new() {
            new Car { Id = 1, Make = "Audi", Model = "R8", Year = 2018, Doors = 2, Color = "Red", Price = 79995 },
            new Car { Id = 2, Make = "Tesla", Model = "3", Year = 2018, Doors = 4, Color = "Black", Price = 54995 },
            new Car { Id = 3, Make = "Porsche", Model = " 911 991", Year = 2020, Doors = 2, Color = "White", Price = 155000 },
            new Car { Id = 4, Make = "Mercedes-Benz", Model = "GLE 63S", Year = 2021, Doors = 5, Color = "Blue", Price = 83995 },
            new Car { Id = 5, Make = "BMW", Model = "X6 M", Year = 2020, Doors = 5, Color = "Silver", Price = 62995 },
        };

        public void AddOrEdit(Car car)
        {
            if (cars.Exists(c => c.Id == car.Id))
            {
                Delete(car.Id);
            }
            else {
                car.Id = cars.Max(c => c.Id) + 1;
            }

            cars.Add(car);
        }

        public void Delete(int id)
        {
            cars.Remove(Find(id).Result);
        }

        public async Task<Car> Find(int id)
        {
            return await Task.Run(() => cars.FirstOrDefault(car => car.Id == id, new Car()));
        }

        public async Task<IList<Car>> Get()
        {
            return await Task.Run(() => cars);
        }
    }
}
