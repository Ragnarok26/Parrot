using Entity.Response;
using Entity.Restaurant;
using Entity.Token;
using Entity.User;
using System;

namespace Logic.Restaurant.Interface
{
    public interface IRestaurant : IDisposable
    {
        Response<TokenData> Login(User user);
        Response<TokenData> Refresh(TokenData tokenData);
        Response<object> Validate(string token);
        Response<StoreData> GetStores(string token);
        Response<Store> GetCategoriesByStore(string token, Store store);
        Response<Product> UpdateProduct(string token, Guid productUuid, string status);
    }
}
