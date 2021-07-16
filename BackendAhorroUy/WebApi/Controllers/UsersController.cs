using BusinessLogic.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using WebApi.DataTypes.ForRequest.UserDTs;
using WebApi.DataTypes.ForResponse.UserDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{

    [ExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserManagement userController;
        public UsersController(IUserManagement controllerUsers) : base()
        {
            userController = controllerUsers;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserPostRequest user)
        {
            var newUser = userController.Create(user.ToEntity());
            var newUserDT = UserPostResponse.ToModel(newUser);
            return Created("User created successfully", newUserDT);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = userController.Get(id);
            return Ok(UserPostResponse.ToModel(user));
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            IEnumerable<User> allUsers = userController.GetAll();
            return Ok(UserPostResponse.ToModel(allUsers));
        }
    }
}