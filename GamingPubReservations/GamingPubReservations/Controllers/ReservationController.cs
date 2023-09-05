using BusinessLayer.Dtos;
using BusinessLayer.Infos;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamingPubReservations.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult AddNewReservation(AddOrUpdateReservationDto addReservationDto)
        {
            int idClaim;
            string roleClaim;

            GetIdAndRole(out idClaim, out roleClaim);

            if (_reservationService.AddReservation(addReservationDto, roleClaim, idClaim))
            {
                return Ok("Reservation added");
            }
            return BadRequest();
        }

        [HttpPut("update/{reservationId}")]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult UpdateReservation([FromBody] AddOrUpdateReservationDto updateReservationDto, [FromRoute] int reservationId)
        {
            int idClaim;
            string roleClaim;

            GetIdAndRole(out idClaim, out roleClaim);

            if (_reservationService.UpdateReservation(updateReservationDto, reservationId, roleClaim, idClaim))
            {
                return Ok("Reservation updated");
            }
            return BadRequest($"Reservation wasn't updated because reservation with id {reservationId} doesn't exist");
        }

        [HttpDelete("delete/{reservationId}")]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult DeleteReservation([FromRoute] int reservationId)
        {
            if (_reservationService.DeleteReservation(reservationId))
            {
                return Ok("Reservation deleted");
            }
            return BadRequest($"Reservation wasn't deleted because reservation with id {reservationId} doesn't exist");
        }

        [HttpGet("by-id/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ReservationInfo> GetById(int id)
        {
            return _reservationService.GetById(id);
        }

        [HttpGet("availables-by-date/{date}/{gamingPubId}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<AvailableReservation>> GetAvailableReservationsByDate([FromRoute] DateTime date, [FromRoute] int gamingPubId)
        {
            return _reservationService.GetAvailablesByDate(date, gamingPubId);
        }

        [HttpGet("availables-by-date-and-platform/{date}/{gamingPlatformId}/{gamingPubId}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<AvailableReservation>> GetAvailableReservationsByDate([FromRoute] DateTime date, [FromRoute] int gamingPlatformId, [FromRoute] int gamingPubId)
        {
            return _reservationService.GetAvailablesByDateAndPlatform(date, gamingPlatformId, gamingPubId);
        }

        [HttpGet("all-reservations")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Reservation>> GetAllReservations()
        {
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }

        [HttpGet("by-date/{date}/{gamingPubId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<ReservationInfo>> GetReservationsInfoByDate([FromRoute] DateTime date, [FromRoute] int gamingPubId)
        {
            return _reservationService.GetByDate(date, gamingPubId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-range-of-days/{startDate}/{endDate}/{gamingPubId}")]
        public ActionResult<List<ReservationInfo>> GetReservationsInfoByDate([FromRoute] DateTime startDate, [FromRoute] DateTime endDate, [FromRoute] int gamingPubId)
        {
            return _reservationService.GetByRange(startDate, endDate, gamingPubId);
        }

        private void GetIdAndRole(out int idClaim, out string roleClaim)
        {
            idClaim = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id").Value);
            roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        }
    }
}