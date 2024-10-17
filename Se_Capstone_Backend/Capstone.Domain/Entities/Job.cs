﻿using Capstone.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Domain.Entities
{
    [Table("jobs")]
    public class Job : BaseEntity

    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;
        public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
    }
}
