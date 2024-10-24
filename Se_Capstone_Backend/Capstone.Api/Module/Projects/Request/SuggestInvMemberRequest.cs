﻿using Capstone.Application.Module.Projects.Query;

namespace Capstone.Api.Module.Projects.Request
{
    public class SuggestInvMemberRequest
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDetail { get; set; } = string.Empty;
        public List<UserStatistic> UserStatistics { get; set; } = new List<UserStatistic>();    
    }
}
