using MiniPOS.Application.Interfaces;
using MiniPOS.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public class QuickBooksCustomerService : IQuickBooksCustomerService
    {
        private readonly HttpClient _client;

        public QuickBooksCustomerService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> CreateCustomerAsync(
            Customer customer,
            string accessToken,
            string realmId)
        {
            var url = $"https://sandbox-quickbooks.api.intuit.com/v3/company/{realmId}/customer";

            var payload = new
            {
                DisplayName = customer.Name,
                PrimaryPhone = new { FreeFormNumber = customer.Phone },
                PrimaryEmailAddr = new { Address = customer.Email }
            };

            return await SendAsync(url, payload, accessToken);
        }

        public async Task<string> UpdateCustomerAsync(
            Customer customer,
            string qbCustomerId,
            string accessToken,
            string realmId)
        {
            var url = $"https://sandbox-quickbooks.api.intuit.com/v3/company/{realmId}/customer?operation=update";

            var payload = new
            {
                Id = qbCustomerId,
                SyncToken = "0", // in production, fetch from QB first
                DisplayName = customer.Name,
                PrimaryPhone = new { FreeFormNumber = customer.Phone },
                PrimaryEmailAddr = new { Address = customer.Email }
            };

            return await SendAsync(url, payload, accessToken);
        }

        private async Task<string> SendAsync(string url, object payload, string token)
        {
            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }


    }
}
