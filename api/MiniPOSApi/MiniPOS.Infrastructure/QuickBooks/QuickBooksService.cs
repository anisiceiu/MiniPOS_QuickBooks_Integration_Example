using Microsoft.Extensions.Options;
using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.QuickBooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public class QuickBooksService : IQuickBooksService
    {
        private readonly QuickBooksHttpClient _client;
        private readonly QuickBooksOptions _options;
        private readonly ITokenRepository _tokenRepo;

        public QuickBooksService(
            QuickBooksHttpClient client,
            IOptions<QuickBooksOptions> options,
            ITokenRepository tokenRepo)
        {
            _client = client;
            _options = options.Value;
            _tokenRepo = tokenRepo;
        }

        public async Task<string> CreateInvoiceAsync(Sale sales)
        {
            var token = await _tokenRepo.GetLatestTokenAsync();

            if (token.IsExpired)
            {
                // refresh automatically
                token = await RefreshToken(token);
            }

            var invoice = MapToInvoice(sales);

            var url = $"{_options.BaseUrl}/{token.RealmId}/invoice";

            var json = JsonConvert.SerializeObject(invoice);

            var result = await _client.PostAsync(url, json, token.AccessToken);

            return result;
        }

        private object MapToInvoice(Sale sales)
        {
            return new
            {
                CustomerRef = new { value = sales.CustomerId },
                Line = sales.SaleItems.Select(x => new
                {
                    Amount = x.LineTotal,
                    DetailType = "SalesItemLineDetail",
                    SalesItemLineDetail = new
                    {
                        Qty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        ItemRef = new { value = x.ProductId }
                    }
                })
            };
        }

        private async Task<TokenEntity> RefreshToken(TokenEntity token)
        {
            // call auth service here (or inject it)
            throw new NotImplementedException();
        }
    }
}
