﻿namespace iRead.API.Models.Account
{
    public class RegistrationForm
    {
        //account info
        public string Username { get; set; }
        public string Password { get; set; }

        //personal info
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? Gender { get; set; }
        public int? IdType { get; set; }
        public string IdNumber { get; set; }


        //contact info
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

    }
}
