using BSRequestHelper.Abstract;
using BSRequestHelper.Helpers;
using BSRequestHelper.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

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
            return HandleResponse<T>(response, JsonConvert.SerializeObject(data));
        }

        public async Task<T> PutAsync<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);
            var request = new RestRequest(url, Method.Put);
            request.AddJsonBody(data);

            var response = await _restClient.ExecuteAsync<T>(request);
            return HandleResponse<T>(response, JsonConvert.SerializeObject(data));
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
        public async Task<T> PostSoapAsync<T>(string url, string soapAction, string soapEnvelope, Dictionary<string, string> headers = null)
        {
            SetHeaders(headers);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "text/xml;charset=UTF-8");
            request.AddHeader("SOAPAction", soapAction);
            request.AddParameter("text/xml", soapEnvelope, ParameterType.RequestBody);

            var response = await _restClient.ExecuteAsync(request);
            return HandleSoapResponse<T>(response);
        }
        public async Task<TResponse> PostSoapAsync<TRequest, TResponse>(string url, string soapAction, TRequest requestObj, Dictionary<string, string> headers = null)
        {
            string soapEnvelope = QueryStringBuilder.CreateSoapEnvelope(requestObj);
            return await PostSoapAsync<TResponse>(url, soapAction, soapEnvelope, headers);
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

        private T HandleResponse<T>(RestResponse response, string requestBody = "")
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                var errorDetail = new BSRequestErrorDetail
                {
                    Url = response.ResponseUri.ToString(),
                    RequestBody = requestBody,
                    Headers = response.Headers.Select(h => $"{h.Name}: {h.Value}").ToList(),
                    ResponseContent = response.Content
                };
                throw new Exception(JsonConvert.SerializeObject(errorDetail));
            }
        }
        private T HandleSoapResponse<T>(RestResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response.Content);

                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            else
            {
                throw new Exception(response.Content);
            }
        }
    }
}
