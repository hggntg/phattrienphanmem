using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Base.APIBuilder
{
    public class API<T>
    {
        public string Method { get; set; }
        public string URI { get; }
        public T Data { get; set; }
        public List<string> Params { get; set; }

        public API(string ApiPath, List<string> Params){
            this.Params = Params;

            int i = 1;
            string NewApiUrl = ApiPath;
            foreach (string d in this.Params)
            {
                NewApiUrl = NewApiUrl.Replace("$" + i, d);
                i++;
            }
            this.URI = NewApiUrl;
        }
    }
    public class Builder
    {
        private readonly HttpClient HttpClientInstance;
        private readonly IMapper Mapper;

        public string ServiceName { get; }

        public Builder(HttpClient HttpClientInstance, IMapper Mapper,  string ServiceName)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.Mapper = Mapper;
            this.ServiceName = ServiceName;
        }

        private string GetRealUri(string Host, string URI)
        {
            if (URI.IndexOf("?") == 0)
            {
                if (Host.LastIndexOf("/") == Host.Length - 1)
                {
                    Host = Host.Substring(0, Host.Length - 1);
                    Host = Host + URI;
                }
                else
                {
                    Host = Host + URI;
                }
            }
            else
            {
                Host = Path.Combine(Host, URI);
            }
            return Host;
        }

        public async Task<U> Put<T, U>(string ApiName, T Data, List<string> Params, AuthenObject Authen)
        {
            API<T> APIInstance = new API<T>(ApiName, Params);
            APIInstance.Method = "PUT";
            APIInstance.Data = Data;

            HttpContent HtppContentInstance = new StringContent(JsonConvert.SerializeObject(APIInstance.Data), Encoding.UTF8, "application/json");
            HttpRequestMessage HttpRequestMessageInstance = new HttpRequestMessage();
            HttpRequestMessageInstance.Method = HttpMethod.Put;
            if (Authen.IsAuthen)
            {
                HttpRequestMessageInstance.Headers.Add("Token", Authen.Token);
            }
            HttpRequestMessageInstance.Content = HtppContentInstance;
            string Host = this.GetRealUri(this.HttpClientInstance.BaseAddress.OriginalString, APIInstance.URI);
            HttpRequestMessageInstance.RequestUri = new Uri(Host);


            var HttpResponse = await this.HttpClientInstance.SendAsync(HttpRequestMessageInstance);
            bool success = HttpResponse.IsSuccessStatusCode;
            if (success)
            {
                string Result = await HttpResponse.Content.ReadAsStringAsync();
                SingleResult<U> LastResult = JsonConvert.DeserializeObject<SingleResult<U>>(Result);
                U ResultObject = LastResult.Result;
                return ResultObject;
            }
            else
            {
                throw new CustomHttpResponseException(new CustomHttpResponseException.ExceptionData((int)HttpResponse.StatusCode, HttpResponse.ReasonPhrase));
            }
        }

        public async Task<U> Post<T,U>(string ApiName, T Data, List<string> Params, AuthenObject Authen)
        {
            API<T> APIInstance = new API<T>(ApiName, Params);
            APIInstance.Method = "POST";
            APIInstance.Data = Data;

            HttpContent HtppContentInstance = new StringContent(JsonConvert.SerializeObject(APIInstance.Data), Encoding.UTF8, "application/json");
            HttpRequestMessage HttpRequestMessageInstance = new HttpRequestMessage();
            HttpRequestMessageInstance.Method = HttpMethod.Post;
            if (Authen.IsAuthen)
            {
                HttpRequestMessageInstance.Headers.Add("Token", Authen.Token);
            }
            HttpRequestMessageInstance.Content = HtppContentInstance;
            string Host = this.GetRealUri(this.HttpClientInstance.BaseAddress.OriginalString, APIInstance.URI);
            HttpRequestMessageInstance.RequestUri = new Uri(Host);


            var HttpResponse = await this.HttpClientInstance.SendAsync(HttpRequestMessageInstance);
            bool success = HttpResponse.IsSuccessStatusCode;
            if (success)
            {
                string Result = await HttpResponse.Content.ReadAsStringAsync();
                SingleResult<U> LastResult = JsonConvert.DeserializeObject<SingleResult<U>>(Result);
                U ResultObject = LastResult.Result;
                return ResultObject;
            }
            else
            {
                throw new CustomHttpResponseException(new CustomHttpResponseException.ExceptionData((int)HttpResponse.StatusCode, HttpResponse.ReasonPhrase));
            }

        }

        public async Task<T> Get<T>(string ApiName, List<string> Params, AuthenObject Authen)
        {
            API<T> APIInstance = new API<T>(ApiName, Params);
            APIInstance.Method = "GET";

            HttpRequestMessage HttpRequestMessageInstance = new HttpRequestMessage();
            HttpRequestMessageInstance.Method = HttpMethod.Get;
            if (Authen.IsAuthen)
            {
                HttpRequestMessageInstance.Headers.Add("Token", Authen.Token);
            }
            string Host = this.GetRealUri(this.HttpClientInstance.BaseAddress.OriginalString, APIInstance.URI);
            
            HttpRequestMessageInstance.RequestUri = new Uri(Host);

            var HttpResponse = await this.HttpClientInstance.SendAsync(HttpRequestMessageInstance);
            bool success = HttpResponse.IsSuccessStatusCode;
            if (success)
            {
                string Result = await HttpResponse.Content.ReadAsStringAsync();
                var ResultObject = JsonConvert.DeserializeObject<SingleResult<T>>(Result);
                T LastResult = ResultObject.Result;
                return LastResult;
            }
            else
            {
                throw new CustomHttpResponseException(new CustomHttpResponseException.ExceptionData((int)HttpResponse.StatusCode, HttpResponse.ReasonPhrase));
            }
        }

        public async Task<List<T>> GetList<T>(string ApiName, List<string> Params, AuthenObject Authen)
        {
            API<T> APIInstance = new API<T>(ApiName, Params);
            APIInstance.Method = "GET";

            HttpRequestMessage HttpRequestMessageInstance = new HttpRequestMessage();
            HttpRequestMessageInstance.Method = HttpMethod.Get;
            if (Authen.IsAuthen)
            {
                HttpRequestMessageInstance.Headers.Add("Token", Authen.Token);
            }
            string Host = this.GetRealUri(this.HttpClientInstance.BaseAddress.OriginalString, APIInstance.URI);
            HttpRequestMessageInstance.RequestUri = new Uri(Host);

            var HttpResponse = await this.HttpClientInstance.SendAsync(HttpRequestMessageInstance);
            bool success = HttpResponse.IsSuccessStatusCode;
            if (success)
            {
                string Result = await HttpResponse.Content.ReadAsStringAsync();
                List<T> LastResult = new List<T>();
                var ResultObject = JsonConvert.DeserializeObject<ListResult<JObject>>(Result);
                ResultObject.Results.ForEach(SingleResult =>
                {
                    LastResult.Add(SingleResult.ToObject<T>());
                });
                return LastResult;
            }
            else
            {
                throw new CustomHttpResponseException(new CustomHttpResponseException.ExceptionData((int)HttpResponse.StatusCode, HttpResponse.ReasonPhrase));
            }
        }
    }

    public class AuthenObject
    {
        public bool IsAuthen { get; set; } = false;
        public string Token { get; set; }
    }

    public class CustomHttpResponseException : Exception
    {
        public ExceptionData CustomData { get; }
        public CustomHttpResponseException(ExceptionData CustomData) : base()
        {
            this.CustomData = CustomData;
        }

        public class ExceptionData
        {
            public int StatusCode { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public ExceptionData(int StatusCode, string Message){
                this.StatusCode = StatusCode;
                this.Message = Message;
                switch(this.StatusCode)
                {
                    case 400:
                        this.Status = "Bad Request";
                        break;
                    case 401:
                        this.Status = "Unauthorized";
                        break;
                    case 403:
                        this.Status = "Forbidden";
                        break;
                    case 404:
                        this.Status = "Not Found";
                        break;
                    default:
                        this.Status = "Unknown";
                        break;
                }
            }
        }
    }
}
