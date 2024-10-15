﻿using Capstone.Application.Common.ResponseMediator;
using Capstone.Application.Module.Labels.Command;
using Capstone.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Labels.CommandHandle
{
    public class UpdateLabelCommandHandle : IRequestHandler<UpdateLabelCommand, ResponseMediator>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateLabelCommandHandle(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseMediator> Handle(UpdateLabelCommand request, CancellationToken cancellationToken)
        {
            var label = _unitOfWork.Labels.Find(x => x.Id == request.Id).FirstOrDefault();
            if(label == null)
                return new ResponseMediator("Label not found", null, 404);
            if(string.IsNullOrEmpty(request.Title))
                return new ResponseMediator("Title empty", null, 400);

            var labelCheckDuplicateTitle = _unitOfWork.Labels.FindOne(x => x.Id != request.Id && x.ProjectId == label.ProjectId && x.Title.Trim().ToUpper() == request.Title.Trim().ToUpper());
            if(labelCheckDuplicateTitle != null)
                return new ResponseMediator("This title label is availble", null, 400);

            label.Title = request.Title;
            label.Description = request.Description;
            _unitOfWork.Labels.Update(label);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseMediator("", label);
        }
    }
}
