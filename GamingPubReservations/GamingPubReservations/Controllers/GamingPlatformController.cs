using BusinessLayer.Dtos;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPubReservations.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class GamingPlatformController : ControllerBase
    {
        private GamingPlatformService _gamingPlatformService;
        public GamingPlatformController(GamingPlatformService gamingPlatformService)
        {
            _gamingPlatformService = gamingPlatformService;
        }

        [HttpGet("all-gaming-platforms")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<GamingPlatform>> GetAllGamingPubs()
        {
            return Ok(_gamingPlatformService.GetAll());
        }

        [HttpGet("get-platform-by-id/{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<GamingPlatform> GetPlatformById(int id)
        {
            return Ok(_gamingPlatformService.GetPlatform(id));
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public ActionResult PostNewPlatform([FromBody] AddGamingPlatformDto gamingPlatform)
        {
            if (_gamingPlatformService.AddGamingPlatform(gamingPlatform))
            {
                return Ok("Gaming platform added");
            }
            return BadRequest("Gaming platform already exists");
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult RemovePlatform([FromBody] IdDto gamingPlatform)
        {
            if (_gamingPlatformService.RemovePlatformById(gamingPlatform))
            {
                return Ok("Gaming platform removed");
            }
            return BadRequest("Gaming platform not found");
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdatePlatform([FromBody] UpdateGamingPlatformDto gamingPlatform)
        {
            if (_gamingPlatformService.UpdatePlatform(gamingPlatform))
            {
                return Ok("Gaming platform updated");
            }
            return BadRequest("Gaming platform not found");
        }
    }
}
