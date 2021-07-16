using BusinessLogic.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.DataTypes.ForResponse.CategoryDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManagement categoryManagement;

        public CategoryController(ICategoryManagement categoryLogic)
        {
            categoryManagement = categoryLogic;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Category> allCategories = categoryManagement.GetAllCategories();
            return Ok(CategoryModel.ToModel(allCategories));
        }
    }
}
