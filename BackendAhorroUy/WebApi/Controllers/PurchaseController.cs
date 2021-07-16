using BusinessLogic.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/purchase")]
    public class PurchaseController : ControllerBase
    {
        private IPurchaseManagement purchaseManagement;
        public PurchaseController(IPurchaseManagement aPurchaseManagement)
        {
            purchaseManagement = aPurchaseManagement;
        }
        [HttpPost]
        public IActionResult SavePurchase([FromHeader] string Auth, [FromBody] Purchase purchase)
        {
            Purchase purchaseSaved = purchaseManagement.SavePurchase(new Guid(Auth), purchase);
            return Ok(purchaseSaved);
        }
    }
}