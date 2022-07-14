using System.ComponentModel.DataAnnotations;

namespace CarApp.Validators {
    public class CarValidator
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Doors")]
        public int Doors { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
    }
}