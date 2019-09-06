﻿using MyLeasing.Common.Models;
using MyLeasing.Common.ViewModels;
using System.Threading.Tasks;

namespace MyLeasing.Common.Services
{
    public interface IApiService
    {
        Task<Response<OwnerResponse>> GetOwnerByEmail(
            string urlBase,
            string servicePrefix, 
            string controller, 
            string tokenType, 
            string accessToken, 
            string email);

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            TokenRequest request);
    }
}