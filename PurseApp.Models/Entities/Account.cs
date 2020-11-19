using System;
using System.ComponentModel.DataAnnotations;

namespace PurseApp.Models
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public Guid PurseId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}