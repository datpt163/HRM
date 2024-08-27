﻿using Capstone.Application.Common.Paging;
using Capstone.Application.Module.Users.Response;
using MediatR;
using System;

namespace Capstone.Application.Module.Users.Query
{
    public class GetUserListQuery : PagingQuery, IRequest<PagingResultSP<UsersDto>>
    {
        public string? FullName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = null;
        public int? Gender { get; set; } = null;
        public DateTime? DobFrom { get; set; } = null;
        public DateTime? DobTo { get; set; } = null;

    }
}
