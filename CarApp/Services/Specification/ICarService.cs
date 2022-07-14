using CarApp.Models;

namespace CarApp.Services.Specification
{
    public interface ICarService
    {
        void AddOrEdit(Car car);
        void Delete(int id);
        Task<Car> Find(int id);
        Task<IList<Car>> Get();
    }
}
