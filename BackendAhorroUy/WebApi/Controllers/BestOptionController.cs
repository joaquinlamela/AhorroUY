using BusinessLogic.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.Filters;
using WebApiModels.DataTypes.MarketDTs;

namespace WebApi.Controllers
{
    [ExceptionFilter]
    [ApiController]
    [Route("api/bestOption")]
    public class BestOptionController : ControllerBase
    {
        private IProductMarketManagemenet productMarketManagement;

        public BestOptionController(IProductMarketManagemenet productMarketLogic) : base()
        {
            productMarketManagement = productMarketLogic;
        }

        [HttpPost]
        [AuthFilter]
        public IActionResult BestMarketOptionForProducts([FromHeader] string Auth, [FromBody] BestOptionRequest[] products, [FromQuery] float longitude, [FromQuery] float latitude, [FromQuery] int maxDistance)
        {
            List<Tuple<Market, double>> bestOption = productMarketManagement.BestOptionToBuy(products, longitude, latitude, maxDistance);
            List<BestOptionResponse> responseList = new List<BestOptionResponse>();
            foreach (Tuple<Market, double> option in bestOption)
            {
                DateTime today = DateTime.Now;
                DateTime openTime = option.Item1.OpeningTime;
                DateTime closeTime = option.Item1.ClosingTime;
                BestOptionResponse responseItem = new BestOptionResponse()
                {
                    MarketName = option.Item1.Name,
                    MarketAddress = option.Item1.Address,
                    PriceForProducts = option.Item2,
                    MarketLatitude = option.Item1.Latitude,
                    MarketLongitude = option.Item1.Longitude,
                    MarketLogo = option.Item1.Logo,
                    OpenTimeToday = new DateTime(today.Year, today.Month, today.Day, openTime.Hour, openTime.Minute, 0),
                    CloseTimeToday = new DateTime(today.Year, today.Month, today.Day, closeTime.Hour, closeTime.Minute, 0)
                };
                responseItem.CalculateDifferenceToCloseInHours();
                responseList.Add(responseItem);
            }
            return Ok(responseList);
        }
    }
}

