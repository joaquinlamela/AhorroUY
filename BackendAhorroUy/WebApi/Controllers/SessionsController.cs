using BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.DataTypes.ForRequest.SessionDTs;
using WebApi.DataTypes.ForResponse.SessionDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private IUserSessionManagement sessionLogic;

        public SessionsController(IUserSessionManagement sessionLogic) : base()
        {
            this.sessionLogic = sessionLogic;
        }

        [HttpPost]
        public IActionResult Login([FromBody] SessionPostRequest userCredentials)
        {
            var token = sessionLogic.Login(userCredentials.ToEntity());
            var userTokenDT = new SessionPostResponse() { Token = token };
            return Created("Successfully logged in. Session token: " + userTokenDT.Token, userTokenDT);
        }

        [HttpDelete("{token}")]
        public IActionResult Logout(string token)
        {
            sessionLogic.Logout(new Guid(token));
            return Ok("Successfully logged out of session " + token);
        }
    }
}