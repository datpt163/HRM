﻿using Capstone.Application.Common.Paging;
using Capstone.Application.Module.Applicants.Query;
using Capstone.Application.Module.Applicants.Response;
using Capstone.Domain.Entities;
using Capstone.Domain.Helpers;
using Capstone.Infrastructure.Helpers;
using Capstone.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Module.Applicants.QueryHandle
{
    public class GetApplicantListQueryHandler : IRequestHandler<GetApplicantListQuery, PagingResultSP<ApplicantDto>>
    {
        private readonly IRepository<Applicant> _applicantRepository;

        public GetApplicantListQueryHandler(IRepository<Applicant> applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        public async Task<PagingResultSP<ApplicantDto>> Handle(GetApplicantListQuery request, CancellationToken cancellationToken)
        {
            var applicantsQuery = _applicantRepository.GetQueryNoTracking()
                .Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(request.Email))
            {
                if (!EmailHelper.IsValidEmail(request.Email))
                {
                    throw new ArgumentException("Invalid email format.");
                }
                applicantsQuery = applicantsQuery.Where(a => a.Email.Contains(request.Email));
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                if (!PhoneNumberValidator.Validate(request.PhoneNumber))
                {
                    throw new ArgumentException("Invalid phone number format.");
                }
                applicantsQuery = applicantsQuery.Where(a => a.PhoneNumber != null && a.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()));

            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                applicantsQuery = applicantsQuery.Where(a => a.Name.Contains(request.Name));
            }

            if (request.IsOnBoard.HasValue)
            {
                applicantsQuery = applicantsQuery.Where(a => a.IsOnBoard == request.IsOnBoard.Value);
            }
            if (request.StartDateFrom.HasValue)
            {
                applicantsQuery = applicantsQuery.Where(a => a.StartDate >= request.StartDateFrom.Value);
            }
            if (request.StartDateTo.HasValue)
            {
                applicantsQuery = applicantsQuery.Where(a => a.StartDate <= request.StartDateTo.Value);
            }

            //if (request.JobIds != null && request.JobIds.Any())
            //{
            //    applicantsQuery = applicantsQuery.Where(a => request.JobIds.Contains(a.MainJobId));
            //}

            if (request.MainJobIds != null && request.MainJobIds.Any())
            {
                applicantsQuery = applicantsQuery.Where(a => request.MainJobIds.Contains(a.MainJobId));
            }

            int totalCount = await applicantsQuery.CountAsync(cancellationToken);
            var pagedApplicants = await applicantsQuery
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new ApplicantDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                    StartDate = a.StartDate,
                    PhoneNumber = a.PhoneNumber,
                    CvLink = a.CvLink,
                    CreatedAt = a.CreatedAt,
                    CreatedBy = a.CreatedBy,
                    UpdatedAt = a.UpdatedAt,
                    UpdatedBy = a.UpdatedBy,
                    IsDeleted = a.IsDeleted,
                }).ToListAsync(cancellationToken);

            return new PagingResultSP<ApplicantDto>(pagedApplicants, totalCount, request.PageIndex, request.PageSize);
        }
    }
}
