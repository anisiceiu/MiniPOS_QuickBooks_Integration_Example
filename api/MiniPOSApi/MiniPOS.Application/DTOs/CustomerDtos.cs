using System;
using System.ComponentModel.DataAnnotations;

namespace MiniPOS.Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? QuickBooksCustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }
    }

    public class UpdateCustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }
    }
}