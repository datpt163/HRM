﻿using Capstone.Application.Common.ResponseMediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Auths.Query
{
    public  class RoleDetailQuery : IRequest<ResponseMediator>
    {
        public Guid Id { get; set; }
    }
}
