using Domain;
using System;

namespace BusinessLogic.Interface
{
    public interface IPurchaseManagement
    {
        Purchase SavePurchase(Guid authToken, Purchase purchaseToSave);
    }
}
