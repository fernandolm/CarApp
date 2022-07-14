using CarApp.Validators;
using Microsoft.AspNetCore.Mvc;

namespace CarApp.Models
{
    [ModelMetadataType(typeof(CarValidator))]
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Doors { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
    }
}