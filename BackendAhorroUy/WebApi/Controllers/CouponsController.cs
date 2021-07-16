using BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using WebApi.DataTypes.ForResponse.CouponDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{

    [ExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private ICouponManagement couponsLogic;
        public CouponsController(ICouponManagement coupons) : base()
        {
            couponsLogic = coupons;
        }

        [HttpGet]
        public IActionResult GetAllFromUser([FromHeader] string Auth)
        {
            var coupons = couponsLogic.GetAllFromUser(new Guid(Auth));
            var couponsDTO = new List<CouponsGetResponse>();
            coupons.ForEach(c => couponsDTO.Add(CouponsGetResponse.ToModel(c)));
            return Ok(couponsDTO);
        }
    }
}