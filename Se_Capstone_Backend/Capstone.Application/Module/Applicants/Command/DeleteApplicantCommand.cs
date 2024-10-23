﻿using Capstone.Application.Module.Applicants.Response;
using MediatR;


namespace Capstone.Application.Module.Applicants.Command
{
    public class DeleteApplicantCommand : IRequest<ApplicantDto?>
    {
        public Guid Id { get; set; }

        public DeleteApplicantCommand(Guid id)
        {
            Id = id;
        }
    }
}
