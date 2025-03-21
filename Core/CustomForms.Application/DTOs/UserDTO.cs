﻿namespace CustomForms.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool LockoutEnabled { get; set; }
        public string SalesforceAccountId { get; set; }
    }
}
