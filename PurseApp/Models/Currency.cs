using System;
using System.ComponentModel.DataAnnotations;

namespace PurseApp.Models
{
    public class Currency
    {
        [Key]
        public Guid CurrencyId { get; set; }
        public string Name { get; set; }
    }
}