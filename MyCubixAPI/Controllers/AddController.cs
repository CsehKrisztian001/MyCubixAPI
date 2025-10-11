using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyCubixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddController : ControllerBase
    {
        /// <summary>
        /// Két egész szám összegét adja vissza ez az API.
        /// </summary>
        /// <param name="number1">Első egész szám.</param>
        /// <param name="number2">Második egész szám.</param>
        /// <returns>Visszakapjuk a két szám összegét.</returns>
        [HttpGet(Name = "GetAdd")]
        public long Get(long number1, long number2)
        {
            return number1 + number2;
        }
    }
}
