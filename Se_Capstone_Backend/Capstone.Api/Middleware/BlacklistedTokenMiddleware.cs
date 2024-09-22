﻿using Capstone.Api.Common.Constant;
using Capstone.Domain.Module.Auth.TokenBlackList;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Capstone.Api.Middleware
{
    public class BlacklistedTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public BlacklistedTokenMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "").Trim();

                if (await _tokenBlacklistService.IsTokenBlacklistedAsync(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token is blacklisted.");
                    return;
                }

                if (IsTokenExpired(token, out var errorMessage))
                {
                    context.Response.StatusCode = Token.TokenExpired;
                    await context.Response.WriteAsync(errorMessage);
                    return;
                }
            }

            await _next(context);
        }

        private bool IsTokenExpired(string token, out string errorMessage)
        {
            errorMessage = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero 
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo < DateTime.UtcNow)
                {
                    errorMessage = "Token has expired.";
                    return true;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                errorMessage = "Token has expired.";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Invalid token: {ex.Message}";
                return true;
            }

            return false;
        }
    }
}
