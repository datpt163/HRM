﻿using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Auths.Command;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Auths.CommandHandle
{
    public class AddRoleForUserCommandHandle : IRequestHandler<AddRoleForUserCommand, ResponseMediator>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AddRoleForUserCommandHandle(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<ResponseMediator> Handle(AddRoleForUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.UserId + "");
            if (user == null)
            {
                return new ResponseMediator("User not found", null);
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId + "");

            if (role == null)
            {
                return new ResponseMediator("Role not found", null);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name ?? "");

            if (result.Succeeded)
            {
                return new ResponseMediator("", null);
            }
            else
            {
                return new ResponseMediator("Fail", null);
            }
        }
    }
}
    