using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PurseApp.Models;
using PurseApp.Models.Dto;

namespace PurseApp.Repositories
{
    public interface IUserRepository
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
        Task<AuthenticateResponse> Register(RegisterRequest user);
        Task DeleteUsers();
    }
}