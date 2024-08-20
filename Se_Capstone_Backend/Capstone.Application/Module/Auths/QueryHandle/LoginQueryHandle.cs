﻿using Capstone.Application.Common.Jwt;
using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Auth.Query;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Capstone.Application.Module.Auth.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Capstone.Application.Module.Auth.QueryHandle
{
    public class LoginQueryHandle : IRequestHandler<LoginQuery, ResponseMediator>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        public LoginQueryHandle(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<ResponseMediator> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null )
            {
                if(await _userManager.CheckPasswordAsync(user, request.Password) )
                {
                    var accessToken = _jwtService.GenerateJwtToken(user, DateTime.Now.AddDays(10));
                    var refreshToken = _jwtService.GenerateJwtToken(user, DateTime.Now.AddDays(30));
                    return new ResponseMediator("", new LoginResponse() { AccessToken = accessToken, RefreshToken = refreshToken });
                }
                return new ResponseMediator("Passwork not correct", null);
            }
            return new ResponseMediator("Account not found", null);
        }
    }
}
