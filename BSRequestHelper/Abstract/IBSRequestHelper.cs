﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSRequestHelper.Abstract
{
    public interface IBSRequestHelper
    {
        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<T> GetAsync<T>(string url, object queryParams, Dictionary<string, string> headers = null);
        Task<T> PostAsync<T>(string url, object data, Dictionary<string, string> headers = null);
        Task<T> PutAsync<T>(string url, object data, Dictionary<string, string> headers = null);
        Task<T> DeleteAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<T> DeleteAsync<T>(string url, object queryParams, Dictionary<string, string> headers = null);
        Task<T> PostSoapAsync<T>(string url, string soapAction, string soapEnvelope, Dictionary<string, string> headers = null);
        Task<TResponse> PostSoapAsync<TRequest, TResponse>(string url, string soapAction, TRequest requestObj, Dictionary<string, string> headers = null);
    }
}
