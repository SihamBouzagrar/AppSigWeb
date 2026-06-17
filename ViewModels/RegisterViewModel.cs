using System.ComponentModel.DataAnnotations;

namespace ReservationHotel.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le nom complet est obligatoire")]
        [Display(Name = "Nom complet")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        [Display(Name = "Confirmer le mot de passe")]
        public string ConfirmPassword { get; set; } = "";
    }
}