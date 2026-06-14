using Microsoft.AspNetCore.Mvc;
using ReservationHotel.Services;
using System.Diagnostics;
using HotelReservation.Models;

namespace HotelReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelService _hotelService;

        public HomeController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET /
        // Page d'accueil : affiche le hero + barre de recherche (pas de liste d'hôtels ici)
        // ✅ FIX: HomeController.Index ne doit PAS retourner la liste des hôtels —
        //         c'est le rôle de HotelsController.Index.
        //         La page d'accueil passe juste les paramètres de recherche à l'API.
        public IActionResult Index()
        {
            return View();
        }

        // GET /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}