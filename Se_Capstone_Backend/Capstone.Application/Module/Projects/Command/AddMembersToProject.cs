﻿using Capstone.Application.Common.ResponseMediator;
using MediatR;


namespace Capstone.Application.Module.Projects.Command
{
    public class AddMembersToProject : IRequest<ResponseMediator>
    {
        public Guid ProjectId { get; set; }
        public List<Guid> MemberIds { get; set; } = new List<Guid>();
    }
}
