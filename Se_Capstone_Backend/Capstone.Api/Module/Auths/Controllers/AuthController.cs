﻿using Capstone.Api.Common.ResponseApi.Controllers;
using Capstone.Api.Common.ResponseApi.Model;
using Capstone.Application.Module.Auth.Command;
using Capstone.Application.Module.Auth.Query;
using Capstone.Application.Module.Auth.Response;
using Capstone.Application.Module.Auths.Command;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone.Api.Module.Auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public AuthController(IMediator mediator, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _mediator = mediator;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [SwaggerResponse(200, "Successful", typeof(ResponseSuccess<LoginResponse>))]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [HttpPost("login")]
        public async Task<IActionResult> Auth([FromBody] LoginQuery loginQuery)
        {
            var result = await _mediator.Send(loginQuery);

            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(dataResponse: result.Data);
            return ResponseBadRequest(messageResponse: result.ErrorMessage);
        }
    }
}
