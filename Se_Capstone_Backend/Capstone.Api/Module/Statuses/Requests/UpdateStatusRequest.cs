﻿namespace Capstone.Api.Module.Statuses.Requests
{
    public class UpdateStatusRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
