using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PaymentApp.Core.Shared.Types;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentApp.UI.Helpers
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient(IConfiguration configuration)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection("AppSettings")["ApiBase"])
            };
         }

        private HttpClient _HttpClient
        {
            get
            {
                return _httpClient;
            }
        }
        public async Task<T> Get<T>(string endPoint)
        {
            if (string.IsNullOrEmpty(endPoint))
                throw new Exception("HTTP endpoint is required");

            var response = await _HttpClient.GetAsync(endPoint);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsAsync<ApiResponse<T>>();
                return result.Result;
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ApiErrorResponse>();
                throw new Exception(error.Message);
            }
        }

        public async Task<TResponse> Post<TResponse, TRequest>(string endPoint, TRequest data)
        {
            var response = await _HttpClient.PostAsJsonAsync<TRequest>(endPoint, data);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponse<TResponse>>();
                return apiResponse.Result;
            }
            else
            {
                //NLog.LogManager.GetCurrentClassLogger().Debug(response);
                var error = await response.Content.ReadAsAsync<ApiErrorResponse>();
                throw new Exception(error.Message);
                // return default(TResponse);
            }
        }

        public async Task<TResponse> Put<TResponse, TRequest>(string endPoint, TRequest data)
        {
            var response = await _HttpClient.PutAsJsonAsync<TRequest>(endPoint, data);
            var apiResp  = await response.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return apiResp.Result;
        }
        public async Task<TResponse> Delete<TResponse>(string endPoint)
        {
            var response = await _HttpClient.DeleteAsync($"{endPoint}");

            var apiResp = await response.Content.ReadAsAsync<ApiResponse<TResponse>>();

            return apiResp.Result;
        }

        public async Task<TResponse> Delete<TResponse>(string endPoint, object id)
        {
            var response = await _HttpClient.DeleteAsync($"{endPoint}{id}");

            var apiResp = await response.Content.ReadAsAsync<ApiResponse<TResponse>>();

            return apiResp.Result;
        }
    }
}
