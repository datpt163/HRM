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
using Capstone.Infrastructure.Repository;
using Capstone.Application.Module.Auths.Response;
using StackExchange.Redis;
using Capstone.Infrastructure.Redis;
using Capstone.Application.Module.Auths.Model;
using CloudinaryDotNet.Actions;

namespace Capstone.Application.Module.Auth.QueryHandle
{
    public class LoginQueryHandle : IRequestHandler<LoginQuery, ResponseMediator>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RedisContext _redis;
        public LoginQueryHandle(UserManager<User> userManager, IJwtService jwtService, IUnitOfWork unitOfWork, RedisContext redis)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _redis = redis;
        }

        public async Task<ResponseMediator> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var accessToken = await _jwtService.GenerateJwtTokenAsync(user, DateTime.Now.AddMinutes(1));
                    var refreshToken = await _jwtService.GenerateJwtTokenAsync(user, DateTime.Now.AddDays(30));
                    user.RefreshToken = refreshToken;
                    await _userManager.UpdateAsync(user);
                    
                    if (roles.FirstOrDefault() != null)
                    {
                        var role = _unitOfWork.Roles.Find(x => x.Name == roles.FirstOrDefault()).FirstOrDefault();
                        if(role != null)
                        {
                            var listCheckToken = _redis.GetData<List<MonitorTokenModel>>("ListMonitorToken");
                            if (listCheckToken != null)
                            {

                                listCheckToken.Add(new MonitorTokenModel() { RoleId = role.Id, Token = accessToken, Status = false });
                                _redis.SetData<List<MonitorTokenModel>>("ListMonitorToken", listCheckToken, DateTime.Now.AddYears(20));
                            }
                            else
                            {
                                _redis.SetData<List<MonitorTokenModel>>("ListMonitorToken", new List<MonitorTokenModel>() { new MonitorTokenModel() { RoleId = role.Id, Token = accessToken, Status = false} }, DateTime.Now.AddYears(20));
                            }

                            return new ResponseMediator("", new LoginResponse()
                            {
                                AccessToken = accessToken,
                                RefreshToken = refreshToken,
                                User = new RegisterResponse(role.Id, role.Name, user.Status, user.Email ?? "", user.Id, user.UserName ?? "", user.FullName, user.PhoneNumber ?? "", user.Avatar ?? "",
                                            user.Address ?? "", user.Gender, user.Dob, user.BankAccount, user.BankAccountName,
                                            user.CreateDate, user.UpdateDate, user.DeleteDate)
                            });
                        }
                    }

                    return new ResponseMediator("", new LoginResponse()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        User = new RegisterResponse(null, null, user.Status, user.Email ?? "", user.Id, user.UserName ?? "", user.FullName, user.PhoneNumber ?? "", user.Avatar ?? "",
                                              user.Address ?? "", user.Gender, user.Dob, user.BankAccount, user.BankAccountName,
                                              user.CreateDate, user.UpdateDate, user.DeleteDate)
                    });
                }
                return new ResponseMediator("Password not correct", null, 400);
            }
            return new ResponseMediator("Account not found", null, 404);
        }
    }
}
