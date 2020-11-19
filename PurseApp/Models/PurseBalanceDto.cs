using AutoMapper;

namespace PurseApp.Models.Dto
{
    [AutoMap(typeof(Account))]
    public class AccountBalance
    {
        public  string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
    }
}