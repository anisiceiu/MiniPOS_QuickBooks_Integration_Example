using Microsoft.EntityFrameworkCore;
using MiniPOS.Application.DTOs;
using MiniPOS.Application.Interfaces;
using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Application.QuickBooks;
using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.QuickBooks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniPOS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbContext _context;
        private readonly IQuickBooksCustomerService _qbService;
        private readonly ITokenRepository _tokenRepo;
        private readonly IQuickBooksAuthService _authService;
        public CustomerService(IApplicationDbContext context, IQuickBooksCustomerService qbService, ITokenRepository tokenRepo, IQuickBooksAuthService authService)
        {
            _context = context;
            _qbService = qbService;
            _tokenRepo = tokenRepo;
            _authService = authService;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) throw new KeyNotFoundException("Customer not found");
            return MapToDto(customer);
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers.Select(MapToDto).ToList();
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            await SyncToQuickBooks(customer);

            return MapToDto(customer);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) throw new KeyNotFoundException("Customer not found");
            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            await _context.SaveChangesAsync();

            await SyncToQuickBooks(customer);

            return MapToDto(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) throw new KeyNotFoundException("Customer not found");
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        private CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                QuickBooksCustomerId = customer.QuickBooksCustomerId,
                CreatedAt = customer.CreatedAt
            };
        }

        private async Task SyncToQuickBooks(Customer customer)
        {
            var token = await _tokenRepo.GetLatestTokenAsync();

            if (token.AccessTokenExpiresAt <= DateTime.UtcNow)
            {
                var tokenResponse = await _authService.RefreshTokenAsync(token.RefreshToken); 
                token.AccessToken = tokenResponse.access_token;
                token.RefreshToken = tokenResponse.refresh_token;
                token.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in);
                await _tokenRepo.UpdateAsync(token);
            }

            string response;

            if (string.IsNullOrEmpty(customer.QuickBooksCustomerId))
            {
                response = await _qbService.CreateCustomerAsync(
                    customer,
                    token.AccessToken,
                    token.RealmId);
            }
            else
            {
                response = await _qbService.UpdateCustomerAsync(
                    customer,
                    customer.QuickBooksCustomerId,
                    token.AccessToken,
                    token.RealmId);
            }

            // TODO: parse response (get QB Customer ID)
            var (id, synctoken) = ParseCustomerResponse(response);
            customer.SyncStatus = "Synced";
            customer.LastSyncedAt = DateTime.UtcNow;
            customer.QuickBooksCustomerId = id;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        private (string CustomerId, string SyncToken) ParseCustomerResponse(string xml)
        {
            var doc = XDocument.Parse(xml);

            XNamespace ns = "http://schema.intuit.com/finance/v3";

            var customer = doc.Descendants(ns + "Customer").FirstOrDefault();

            if (customer == null)
                throw new Exception("Customer node not found.");

            var id =
                customer.Element(ns + "Id")?.Value;

            var syncToken =
                customer.Element(ns + "SyncToken")?.Value;

            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("QuickBooks customer ID missing.");

            return (id, syncToken);
        }
    }
}