﻿using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Auths.Command;
using Capstone.Domain.Entities;
using Capstone.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Auths.CommandHandle
{
    public class UpdateRoleCommandHandle : IRequestHandler<UpdateRoleCommand, ResponseMediator>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleCommandHandle(RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseMediator> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return new ResponseMediator("Role name empty", null,400);
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                return new ResponseMediator("Description empty", null,400);
            }

            var roleUpdate = await _unitOfWork.Roles.Find(x => x.Id == request.Id).Include(c => c.Permissions).FirstOrDefaultAsync();
            if(roleUpdate == null)
                return new ResponseMediator("Role not found", null,404);

            if ( _unitOfWork.Roles.FindOne( x => x.Name != null && x.Name == request.Name.ToUpper() && x.Id != request.Id ) != null)
            {
                return new ResponseMediator("This role name already exists", null);
            }

            var role = await _roleManager.FindByNameAsync(roleUpdate.Name ?? "");

            if(role == null)
            {
                return new ResponseMediator("Failed to create role", null);
            }
            else
            {
                role.Name = request.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    var createdRole = await _unitOfWork.Roles.Find(c => c.Name == request.Name.ToUpper()).Include(c => c.Permissions).FirstOrDefaultAsync();
                    if (createdRole != null)
                    {
                        createdRole.Description = request.Description;
                        createdRole.Permissions = new List<Permission>();
                        foreach (var p in request.PermissionsId)
                        {
                            var permission = await _unitOfWork.Permissions.FindOneAsync(c => c.Id == p);
                            if (permission != null)
                                createdRole.Permissions.Add(permission);
                        }
                        _unitOfWork.Roles.Update(createdRole);
                        await _unitOfWork.SaveChangesAsync();
                        return new ResponseMediator("", new { id = createdRole.Id, name = createdRole.Name, description = createdRole.Description, permissions = createdRole.Permissions });
                    }
                    return new ResponseMediator("Failed to update role", null);
                }
                else
                {
                    return new ResponseMediator("Failed to update role", null);
                }
            }
        }
    }
}