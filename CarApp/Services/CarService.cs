using CarApp.Repository.Specification;
using CarApp.Models;
using CarApp.Services.Specification;

namespace CarApp.Service
{
    public class CarService : ICarService
    {
        private readonly ICarRepository carRepository;

        public CarService(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public void AddOrEdit(Car car)
        {
            carRepository.AddOrEdit(car);
        }

        public void Delete(int id)
        {
            carRepository.Delete(id);
        }

        public async Task<Car> Find(int id)
        {
            return await carRepository.Find(id);
        }

        public async Task<IList<Car>> Get()
        {
            return await carRepository.Get();
        }
    }
}