using BusinessLayer.Dtos;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPubReservations.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class GamingPubController : ControllerBase
    {
        private GamingPubService _gamingPubService;
        public GamingPubController(GamingPubService gamingPubService)
        {
            _gamingPubService = gamingPubService;
        }

        [HttpGet("all-gaming-pubs")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<GamingPub>> GetAllGamingPubs()
        {
            return Ok(_gamingPubService.GetAll());
        }

        [HttpGet("gaming-pub-by-id/{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<GamingPub> GetGamingPubById(int id)
        {
            return Ok(_gamingPubService.GetPub(id));
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewPub([FromBody] AddGamingPubDto gamingPub)
        {
            if (_gamingPubService.AddGamingPub(gamingPub))
            {
                return Ok("Gaming pub added");
            }
            return BadRequest("Gaming pub already exists");
        }

        [HttpPost("update/{gamingPubId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGamingPub([FromRoute] int gamingPubId, [FromBody] UpdateGamingPubDto updateGamingPub)
        {
            if (_gamingPubService.UpdateGamingPub(gamingPubId, updateGamingPub))
            {
                return Ok("Gaming pub updated");
            }
            return BadRequest("Gaming pub was not updated");
        }

        [HttpDelete("delete/{gamingPubId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteGamingPub([FromRoute] int gamingPubId)
        {
            if (_gamingPubService.DeleteGamingPub(gamingPubId))
            {
                return Ok("Gaming pub updated");
            }
            return BadRequest("Gaming pub was not updated");
        }
    }
}