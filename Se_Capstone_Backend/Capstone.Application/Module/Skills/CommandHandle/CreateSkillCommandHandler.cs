﻿using Capstone.Domain.Entities;
using Capstone.Application.Module.Skills.Command;
using Capstone.Application.Module.Skills.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Infrastructure.Repository;
using MediatR;

namespace Capstone.Application.Module.Skills.CommandHandle
{
    public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, SkillDto>
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSkillCommandHandler(IRepository<Skill> skillRepository, IUnitOfWork unitOfWork)
        {
            _skillRepository = skillRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SkillDto> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = new Skill
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = null,
                IsDeleted = false
            };

             _skillRepository.Add(skill);
            await _unitOfWork.SaveChangesAsync();
            return new SkillDto
            {
                Id = skill.Id,
                Title = skill.Title,
                Description = skill.Description,
                CreatedAt = skill.CreatedAt,
                UpdatedAt = skill.UpdatedAt,
                IsDeleted = skill.IsDeleted,
                CreatedBy = skill.CreatedBy,
                UpdatedBy = skill.UpdatedBy
            };
        }
    }
}
