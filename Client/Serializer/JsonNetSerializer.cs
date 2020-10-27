using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Serializer
{
    public class JsonNetSerializer : RestSharp.Serialization.IRestSerializer
    {
        public string Serialize(object obj) =>
            Newtonsoft.Json.JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response) =>
            Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);

        public string[] SupportedContentTypes { get; } = {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}
