using Microsoft.AspNetCore.Mvc;
using ReservationHotel.DTOs;
using ReservationHotel.Services;

namespace ReservationHotel.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsApiController : ControllerBase
    {
        private readonly HotelService _hotelService;

        public HotelsApiController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET api/hotels/search?city=Marrakech&checkIn=2026-07-01&checkOut=2026-07-05&guests=2
        [HttpGet("search")]
        public async Task<ActionResult<List<HotelSearchResultDto>>> Search(
            [FromQuery] string city,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut,
            [FromQuery] int guests)
        {
            // ✅ FIX: validation AVANT d'appeler le service (était après dans l'original)
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(new { error = "Le champ 'city' est requis." });

            if (checkIn < DateTime.Today)
                return BadRequest(new { error = "La date d'arrivée ne peut pas être dans le passé." });

            if (checkIn >= checkOut)
                return BadRequest(new { error = "La date de départ doit être après la date d'arrivée." });

            if (guests <= 0)
                return BadRequest(new { error = "Le nombre de guests doit être supérieur à 0." });

            var results = await _hotelService.SearchHotelsAsync(city, checkIn, checkOut, guests);

            return Ok(results);
        }

        // GET api/hotels/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HotelDetailsDto>> GetById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);

            if (hotel == null)
                return NotFound(new { error = $"Hôtel avec l'id {id} introuvable." });

            return Ok(hotel);
        }

        // GET api/hotels
        [HttpGet]
        public async Task<ActionResult<List<HotelDetailsDto>>> GetAll()
        {
            var hotels = await _hotelService.GetAllHotelDtosAsync();
            return Ok(hotels);
        }
    }
}