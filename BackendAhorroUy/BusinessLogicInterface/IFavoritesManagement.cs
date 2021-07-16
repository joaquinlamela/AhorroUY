using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Interface
{
    public interface IFavoritesManagement
    {
        Product AddProduct(Guid token, Guid productId);

        List<Product> GetAllFromUser(Guid token);

        void DeleteProduct(Guid token, Guid productId);
    }
}
