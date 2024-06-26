﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionTags")]
    [PrimaryKey(nameof(ContributionId),nameof(TagId))]
    public class ContributionTag 
    {
        public Guid ContributionId { get; set; }
        public Guid TagId { get; set; }
    }
}
