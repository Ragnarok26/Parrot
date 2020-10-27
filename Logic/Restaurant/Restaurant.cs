using Entity.Response;
using Entity.Restaurant;
using Entity.Token;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Restaurant
{
    public class Restaurant : Interface.IRestaurant
    {
        public void Dispose()
        {
        }

        public Response<TokenData> Login(User user)
        {
            Response<TokenData> response = null;
            using (var data = new Data.Restaurant.Restaurant())
            {
                response = data.Login(user);
            }
            if (!string.IsNullOrEmpty(response.Message))
            {
                response.Status = string.Empty;
            }
            return response;
        }

        public Response<TokenData> Refresh(TokenData tokenData)
        {
            Response<TokenData> response = null;
            using (var data = new Data.Restaurant.Restaurant())
            {
                response = data.Refresh(tokenData);
            }
            if (!string.IsNullOrEmpty(response.Message))
            {
                response.Status = string.Empty;
            }
            return response;
        }

        public Response<object> Validate(string token)
        {
            using (var data = new Data.Restaurant.Restaurant())
            {
                return data.Validate(token);
            }
        }

        public Response<StoreData> GetStores(string token)
        {
            using (var data = new Data.Restaurant.Restaurant())
            {
                return data.GetStores(token);
            }
        }

        public Response<Store> GetCategoriesByStore(string token, Store store)
        {
            Response<IEnumerable<Product>> responseProducts = new Response<IEnumerable<Product>>();
            using (var data = new Data.Restaurant.Restaurant())
            {
                responseProducts = data.GetAllProducts(token, store.Uuid);
            }
            Response<Store> response = new Response<Store>
            {
                Message = responseProducts.Message,
                Status = responseProducts.Status
            };
            if (responseProducts.Success)
            {
                try
                {
                    if (responseProducts.Results.Any())
                    {
                        store.Categories = responseProducts.Results
                                           .GroupBy(
                                               product => product.Category.Uuid
                                           )
                                           .Select(category => category.First().Category);
                        if (store.Categories.Any())
                        {
                            foreach (var category in store.Categories)
                            {
                                category.Products = responseProducts.Results
                                                    .Where(product => product.Category.Uuid == category.Uuid);
                            }
                        }
                        response.Result = store;
                    }
                    else
                    {
                        response.Result = new Store();
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    response.Status = string.Empty;
                }
            }
            return response;
        }

        public Response<Product> UpdateProduct(string token, Guid productUuid, string status)
        {
            using (var data = new Data.Restaurant.Restaurant())
            {
                return data.UpdateProduct(token, productUuid, status == "AVAILABLE" ? "UNAVAILABLE" : "AVAILABLE");
            }
        }
    }
}
