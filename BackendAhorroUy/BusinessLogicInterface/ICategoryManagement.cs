using Domain;
using System.Collections.Generic;

namespace BusinessLogic.Interface
{
    public interface ICategoryManagement
    {
        List<Category> GetAllCategories();
    }
}
