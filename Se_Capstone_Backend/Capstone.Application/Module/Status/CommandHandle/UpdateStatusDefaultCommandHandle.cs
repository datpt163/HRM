﻿using Capstone.Application.Common.FileService;
using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Status.Command;
using Capstone.Infrastructure.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Capstone.Domain.Entities;

namespace Capstone.Application.Module.Status.CommandHandle
{
    public class UpdateStatusDefaultCommandHandle : IRequestHandler<UpdateStatusDefaultCommand, ResponseMediator>
    {
        private readonly IFileService _fileService;

        public UpdateStatusDefaultCommandHandle(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<ResponseMediator> Handle(UpdateStatusDefaultCommand request, CancellationToken cancellationToken)
        {
            var statuses = JsonSerializer.Deserialize<List<Domain.Entities.Status>>(await _fileService.ReadFileAsync("Module\\Projects\\Default\\DefaultStatus.json")) ?? new List<Domain.Entities.Status>();
            var status = statuses.FirstOrDefault(x => x.Id == request.Id);
            if (status == null)
                return new ResponseMediator("Status not found", null, 404);
            if (string.IsNullOrEmpty(request.Name))
                return new ResponseMediator("Name empty", null, 400);

            if (string.IsNullOrEmpty(request.Color))
                return new ResponseMediator("Color empty", null, 400);

            var statusCheckDuplicateTitle = statuses.FirstOrDefault(x => x.Id != request.Id && x.ProjectId == status.ProjectId && x.Name.Trim().ToUpper() == request.Name.Trim().ToUpper());
            if (statusCheckDuplicateTitle != null)
                return new ResponseMediator("This name status is availble", null, 400);

            status.Name = request.Name;
            status.Description = request.Description;
            status.Color = request.Color;
            await _fileService.WriteFileAsync("Module\\Projects\\Default\\DefaultStatus.json", JsonSerializer.Serialize(statuses));
            return new ResponseMediator("", status);
        }
    }
}