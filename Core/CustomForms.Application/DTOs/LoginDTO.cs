﻿namespace CustomForms.Application.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
