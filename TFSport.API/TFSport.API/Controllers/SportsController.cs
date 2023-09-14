using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.Entities;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.Controllers
{
    [Route("sports")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly ISportsService _sportsService;

        public SportsController(ISportsService sportsService)
        {
            _sportsService = sportsService;
        }

        /// <summary>
        /// Get list of sport types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, "Request_Succeeded", typeof(List<GetSportsDTO>))]
        public async Task<IActionResult> GetSportTypes()
        {
            var list = await _sportsService.GetSportTypes();
            return Ok(list);
        }


    }
}
