using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;

namespace Client
{
    public class ApiClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="requestFormat"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public T GetServiceData<T, U>(string service, Method method, DataFormat requestFormat = DataFormat.Json, U data = default, string token = null)
        {
            IRestClient client = new RestClient($"{service}");
            if (!string.IsNullOrEmpty(token))
            {
                client.Authenticator = new JwtAuthenticator(token);
            }
            IRestRequest request = new RestRequest(method)
            {
                RequestFormat = requestFormat
            };
            switch (method)
            {
                case Method.POST:
                case Method.PUT:
                    if (!EqualityComparer<U>.Default.Equals(data, default))
                    {
                        switch (requestFormat)
                        {
                            case DataFormat.Json:
                                client.UseSerializer<Serializer.JsonNetSerializer>();
                                request.AddJsonBody(data);
                                break;
                            case DataFormat.Xml:
                                request.AddXmlBody(data);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            IRestResponse<T> resp = client.Execute<T>(request);
            return resp.Data;
        }
    }
}
