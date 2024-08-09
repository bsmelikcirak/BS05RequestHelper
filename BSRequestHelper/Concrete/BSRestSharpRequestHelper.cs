using BSRequestHelper.Abstract;
using BSRequestHelper.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSRequestHelper.Concrete
{
    public class BSRestSharpRequestHelper : IBSRestSharpRequestHelper
    {
        readonly RestClient _restClient;
        public BSRestSharpRequestHelper()
        {
            _restClient = new RestClient();
        }
        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);
            var request = new RestRequest(url, Method.Get);
            var response = await _restClient.ExecuteAsync<T>(request);
            return HandleResponse<T>(response);
        }
        public async Task<T> GetAsync<T>(string url, object queryParams, Dictionary<string, string> headers = null)
        {
            string queryString = QueryStringBuilder.QueryStringFromObject(queryParams);
            string fullUrl = $"{url}?{queryString}";
            return await GetAsync<T>(fullUrl, headers);
        }
        public async Task<T> PostAsync<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(data);

            var response = await _restClient.ExecuteAsync<T>(request);
            return HandleResponse<T>(response);
        }

        public async Task<T> PutAsync<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);
            var request = new RestRequest(url, Method.Put);
            request.AddJsonBody(data);

            var response = await _restClient.ExecuteAsync<T>(request);
            return HandleResponse<T>(response);
        }

        public async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);
            var request = new RestRequest(url, Method.Delete);

            var response = await _restClient.ExecuteAsync<T>(request);
            return HandleResponse<T>(response);
        }

        public async Task<T> DeleteAsync<T>(string url, object queryParams, Dictionary<string, string> headers = null)
        {
            string queryString = QueryStringBuilder.QueryStringFromObject(queryParams);
            string fullUrl = $"{url}?{queryString}";
            return await DeleteAsync<T>(fullUrl, headers);
        }



        private void SetHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _restClient.AddDefaultHeader(header.Key, header.Value);
                }
            }
        }

        private T HandleResponse<T>(RestResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new Exception(response.Content);
            }
        }
    }
}
