using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FunctionalTests.Setup
{
    public class JsonSerialiser<T>
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
        };
        private readonly Encoding _encoding;

        public JsonSerialiser(Encoding encoding)
        {
            _encoding = encoding;
        }

        public byte[] Serialise(T model)
        {
            string strModel = JsonConvert.SerializeObject(model, _jsonSerializerSettings);
            byte[] byteArrModel = _encoding.GetBytes(strModel);
            return byteArrModel;
        }

        public T Deserialise(byte[] byteArr, Action<string> logJson = null)
        {
            string strJson = _encoding.GetString(byteArr);
            logJson?.Invoke(strJson);
            T model = JsonConvert.DeserializeObject<T>(strJson, _jsonSerializerSettings);
            return model;
        }
    }
}
