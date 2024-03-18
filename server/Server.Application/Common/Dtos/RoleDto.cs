using AutoMapper;
using Server.Domain.Entity.Identity;

namespace Server.Application.Common.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppRole, RoleDto>();
        }
    }
}