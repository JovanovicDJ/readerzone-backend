﻿using readerzone_api.Data;
using readerzone_api.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class Customer : User
    {
        public Tier Tier { get; set; }
        public double Points { get; set; } = 0.0;
        public int AnnualChallenge { get; set; } = 1;
        public int AnnualChallengeProgress { get; set; } = 0;
        [JsonIgnore]
        public ICollection<Customer> Friends { get; set; } = new List<Customer>();
        [JsonIgnore]
        public ICollection<PurchasedBook> PurchasedBooks { get; set; } = new List<PurchasedBook>();
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Customer(string name, string surname, DateTime dob, string phoneNumber) : base(name, surname, dob, phoneNumber)
        {
        }

        public Customer(CustomerDto customerDto)
        {
            DateTime dob = DateTime.ParseExact(customerDto.Dob, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None);
            Name = customerDto.Name;
            Surname = customerDto.Surname;
            Dob = dob;
            PhoneNumber = customerDto.PhoneNumber;
            Tier = Tier.Bronze;
            Points = 0;
            AnnualChallenge = 0;
            UserAccount = new UserAccount(customerDto.Username, customerDto.Email, Role.Customer, false, false);
            Address = new Address(customerDto.Street, customerDto.Number, customerDto.City, customerDto.PostalCode, customerDto.Country);           
        }
    }
}
