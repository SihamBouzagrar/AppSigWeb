using System.ComponentModel.DataAnnotations;

namespace ReservationHotel.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; } = false;
    }
}