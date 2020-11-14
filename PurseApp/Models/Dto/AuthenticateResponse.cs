using System;

namespace PurseApp.Models.Dto
{
    public class AuthenticateResponse : IJwtToken
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}