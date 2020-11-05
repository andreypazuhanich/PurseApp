using System;
using System.Collections;
using System.Collections.Generic;

namespace PurseApp.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public IEnumerable<Purse> Purses { get; set; }
        public string Name { get; set; }
        public string INN { get; set; }
    }
}