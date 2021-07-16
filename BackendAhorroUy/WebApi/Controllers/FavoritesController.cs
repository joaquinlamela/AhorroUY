using BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using WebApi.DataTypes.ForResponse.FavoriteDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private IFavoritesManagement favoritesLogic;
        private IProductMarketManagemenet productMarketManagement;

        public FavoritesController(IFavoritesManagement favorites, IProductMarketManagemenet productMarketLogic) : base()
        {
            this.favoritesLogic = favorites;
            this.productMarketManagement = productMarketLogic;
        }
    
        [HttpPost]
        public IActionResult Post([FromHeader] string Auth, [FromQuery] string productId)
        {
            var product = favoritesLogic.AddProduct(new Guid(Auth), new Guid(productId));
            var minAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(new Guid(productId));
            var response = FavoritesPostResponse.ToModel(product);
            response.MinPrice = minAndMaxPrice.Item1;
            response.MaxPrice = minAndMaxPrice.Item2;
            return Created("Product added successfully to favorites", response);
        }
       
        [HttpGet]
        public IActionResult GetAll([FromHeader] string Auth)
        {
            var favorites = favoritesLogic.GetAllFromUser(new Guid(Auth));
            var favoritesDTO = new List<FavoritesGetResponse>();
            favorites.ForEach(f =>
            {
                var MinAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(f.Id);
                var fav = FavoritesGetResponse.ToModel(f);
                fav.MinPrice = MinAndMaxPrice.Item1;
                fav.MaxPrice = MinAndMaxPrice.Item2;
                favoritesDTO.Add(fav);
            });
            return Ok(favoritesDTO);
        }
  
        [HttpDelete]
        public IActionResult Delete([FromHeader] string Auth, [FromQuery] string productId)
        {
            favoritesLogic.DeleteProduct(new Guid(Auth), new Guid(productId));
            return Ok("Product removed successfully");
        }
    }
}