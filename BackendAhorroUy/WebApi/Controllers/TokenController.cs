using BusinessLogicForPushNotification.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ExceptionFilter]
    [Route("api/tokens")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly IPushNotificationManagement tokenManagement;

        public TokenController(IPushNotificationManagement pushNotificationManagement)
        {
            tokenManagement = pushNotificationManagement;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string token)
        {
            Token tokenToAdd = new Token() { TokenValue = token };
            Token tokenCreated = tokenManagement.Add(tokenToAdd);
            return Ok(tokenCreated);
        }
    }
}
