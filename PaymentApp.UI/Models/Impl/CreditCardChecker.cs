using Microsoft.Extensions.Configuration;
using PaymentApp.UI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentApp.UI.Models.Impl
{
    public class CreditCardChecker : ICreditCardChecker
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly string _apiBase;
        public CreditCardChecker(IConfiguration configuration)
        {

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection("AppSettings")["CCEndpoint"])
            };

            _apiBase = configuration.GetSection("AppSettings")["CCEndpoint"];
            _apiKey = configuration.GetSection("AppSettings")["CCKey"];
        }
        public async Task<bool> CheckCard(string cc)
        {
            try
            {
                var details = await GetCardDetails(cc);
                if(details != null)
                {
                    return details.valid.Equals("true");
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<CCResponse> GetCardDetails(string cc)
        {
            CCResponse cardDetails = new CCResponse();
            CCError error;

            if (string.IsNullOrEmpty(cc))
            {
                return null;
            }

            HttpResponseMessage response = null;

            try
            {
                response = await _httpClient.GetAsync($"{_apiBase}/json/{_apiKey}/{cc}");
               

                cardDetails = await response.Content.ReadAsAsync<CCResponse>();
            }
            catch(Exception ex)
            {
                if (response != null)
                {
                    error = await response.Content.ReadAsAsync<CCError>();
                    throw new Exception(error.message);
                }
            }

            return cardDetails;

        }
    }
}
