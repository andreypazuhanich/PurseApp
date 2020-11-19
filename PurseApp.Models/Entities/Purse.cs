using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurseApp.Models
{
    public class Purse
    {
        [Key]
        public Guid PurseId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
    }
}