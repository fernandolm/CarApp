using CarApp.Models;

namespace CarApp.Repository.Specification
{
    public interface ICarRepository
    {
        void AddOrEdit(Car car);
        void Delete(int id);
        Task<Car> Find(int id);
        Task<IList<Car>> Get();
    }
}
