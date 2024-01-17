﻿namespace API.DTOs
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        //public string PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }

    }
}