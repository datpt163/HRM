﻿using Capstone.Domain.Entities;
using Capstone.Application.Module.Jobs.Query;
using Capstone.Application.Module.Jobs.Response;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Capstone.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Application.Module.Jobs.QueryHandle
{
    public class GetJobQueryHandler : IRequestHandler<GetJobQuery, JobDto?>
    {
        private readonly IRepository<Job> _jobRepository;

        public GetJobQueryHandler(IRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<JobDto?> Handle(GetJobQuery request, CancellationToken cancellationToken)
        {
            var job =  await _jobRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (job == null)
            {
                return null;
            }

            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                CreatedAt = job.CreatedAt,
                UpdateAt = job.UpdatedAt,
                IsDeleted = job.IsDeleted,
                CreatedBy = job.CreatedBy,
                UpdatedBy = job.UpdatedBy
            };
        }
    }
}
