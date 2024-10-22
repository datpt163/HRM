﻿using Capstone.Application.Common.FileService;
using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Status.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Status.CommandHandle
{
    public class CreateStatusDefaultCommandHandle : IRequestHandler<CreateStatusDefaultCommand, ResponseMediator>
    {
        private readonly IFileService _fileService;

        public CreateStatusDefaultCommandHandle(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<ResponseMediator> Handle(CreateStatusDefaultCommand request, CancellationToken cancellationToken)
        {
            var statuses = JsonSerializer.Deserialize<List<Domain.Entities.Status>>(await _fileService.ReadFileAsync("Module\\Projects\\Default\\DefaultStatus.json")) ?? new List<Domain.Entities.Status>();
            if (string.IsNullOrEmpty(request.Name))
                return new ResponseMediator("Name empty", null, 400);

            if (string.IsNullOrEmpty(request.Color))
                return new ResponseMediator("Color empty", null, 400);

            if (statuses.Select(x => x.Name.Trim().ToUpper()).Contains(request.Name.Trim().ToUpper()))
                return new ResponseMediator("This name status is availble", null, 400);

            var position = 0;
            if (statuses.Count() != 0)
            {
                position = statuses.Count();
            }

            var status = new Capstone.Domain.Entities.Status() { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description, Color = request.Color, Position = position };
            statuses.Add(status);
            await _fileService.WriteFileAsync("Module\\Projects\\Default\\DefaultStatus.json", JsonSerializer.Serialize(statuses));
            return new ResponseMediator("", new { ID = status.Id, Name = status.Name, Description = request.Description, Color = request.Color, Position = position } );
        }
    }
}