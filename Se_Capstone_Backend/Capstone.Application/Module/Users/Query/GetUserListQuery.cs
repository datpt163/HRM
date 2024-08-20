﻿using Capstone.Application.Module.Users.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Users.Query
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {
    }
}
