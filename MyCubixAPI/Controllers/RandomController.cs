using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyCubixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomController : ControllerBase
    {
        /// <summary>
        /// Egy véletlenszámot generál 1 és az általunk megadott maximum között.
        /// </summary>
        /// <param name="max">Maximum aminek az értékét még felveheti.</param>
        /// <returns>Visszakapunk egy generált véletlen számot.</returns>
        [HttpGet(Name = "GetRandom")]
        public int Get(int max)
        {
            var random = new Random();
            return random.Next(1, max + 1);
        }
    }
}
