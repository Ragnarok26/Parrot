using Client;
using Entity.Response;
using Entity.Restaurant;
using Entity.Token;
using Entity.User;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Data.Restaurant
{
    public class Restaurant : Interface.IRestaurant
    {
        private string baseUrl = ConfigurationManager.AppSettings.Get("baseUrl");
        private string loginUrl = ConfigurationManager.AppSettings.Get("loginUrl");
        private string refreshUrl = ConfigurationManager.AppSettings.Get("refreshUrl");
        private string validateUrl = ConfigurationManager.AppSettings.Get("validateUrl");
        private string storesUrl = ConfigurationManager.AppSettings.Get("storesUrl");
        private string getProductsUrl = ConfigurationManager.AppSettings.Get("getProductsUrl");
        private string updateProductUrl = ConfigurationManager.AppSettings.Get("updateProductUrl");

        public void Dispose()
        {
        }

        public Response<TokenData> Login(User user)
        {
            Response<TokenData> response = new Response<TokenData>();
            try
            {
                using (var client = new ApiClient())
                {
                    response.Result =
                    client.GetServiceData<TokenData, User>(
                        $"{baseUrl}{loginUrl}",
                        Method.POST,
                        DataFormat.Json,
                        user);
                    response.Status = "ok";
                    response.Message = response.Result.Detail;
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<TokenData> Refresh(TokenData tokenData)
        {
            Response<TokenData> response = new Response<TokenData>();
            try
            {
                using (var client = new ApiClient())
                {
                    response.Result =
                    client.GetServiceData<TokenData, dynamic>(
                        $"{baseUrl}{refreshUrl}",
                        Method.POST,
                        DataFormat.Json,
                        new { refresh = tokenData.Refresh });
                    response.Status = "ok";
                    response.Message = response.Result.Detail;
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<object> Validate(string token)
        {
            Response<object> response = new Response<object>();
            try
            {
                using (var client = new ApiClient())
                {
                    response =
                    client.GetServiceData<Response<object>, object>(
                        $"{baseUrl}{validateUrl}",
                        Method.GET,
                        DataFormat.Json,
                        null,
                        token);
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<StoreData> GetStores(string token)
        {
            Response<StoreData> response = new Response<StoreData>();
            try
            {
                using (var client = new ApiClient())
                {
                    response =
                    client.GetServiceData<Response<StoreData>, object>(
                        $"{baseUrl}{storesUrl}",
                        Method.GET,
                        DataFormat.Json,
                        null,
                        token);
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<IEnumerable<Product>> GetAllProducts(string token, Guid storeUuid)
        {
            Response<IEnumerable<Product>> response = new Response<IEnumerable<Product>>();
            try
            {
                using (var client = new ApiClient())
                {
                    response =
                    client.GetServiceData<Response<IEnumerable<Product>>, object>(
                        $"{baseUrl}{getProductsUrl}{storeUuid}",
                        Method.GET,
                        DataFormat.Json,
                        null,
                        token);
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<Product> UpdateProduct(string token, Guid productUuid, string status)
        {
            Response<Product> response = new Response<Product>();
            try
            {
                using (var client = new ApiClient())
                {
                    response =
                    client.GetServiceData<Response<Product>, dynamic>(
                        $"{baseUrl}{string.Format(updateProductUrl, productUuid)}",
                        Method.PUT,
                        DataFormat.Json,
                        new { availability = status },
                        token);
                }
            }
            catch (Exception ex)
            {
                response.Status = string.Empty;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
