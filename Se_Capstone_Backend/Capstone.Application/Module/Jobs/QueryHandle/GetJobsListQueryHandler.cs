﻿using Capstone.Domain.Entities;
using Capstone.Application.Module.Jobs.Query;
using Capstone.Application.Module.Jobs.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Infrastructure.Repository;
using MediatR;

namespace Capstone.Application.Module.Jobs.QueryHandle
{
    public class GetJobsListQueryHandler : IRequestHandler<GetJobsListQuery, List<JobDto>>
    {
        private readonly IRepository<Job> _jobRepository;

        public GetJobsListQueryHandler(IRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobDto>> Handle(GetJobsListQuery request, CancellationToken cancellationToken)
        {
            var query = _jobRepository.GetQuery().AsQueryable();

            if (request.IsDeleted != null)
            {
                query = query.Where(x => x.IsDeleted == request.IsDeleted);
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title.ToLower().Contains(request.Title.ToLower()));

            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(x => x.Description.ToLower().Contains(request.Description.ToLower()));
            }


            var jobs = query.ToList();

            return jobs.Select(job => new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                CreatedAt = job.CreatedAt,
                UpdateAt = job.UpdateAt,
                IsDeleted = job.IsDeleted,
                CreatedBy = job.CreatedBy,
                UpdatedBy = job.UpdatedBy
            }).ToList();
        }
    }
}
