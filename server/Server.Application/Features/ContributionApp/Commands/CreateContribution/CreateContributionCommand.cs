﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Server.Application.Common.Dtos;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Files { get; set; }
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
    }
}
