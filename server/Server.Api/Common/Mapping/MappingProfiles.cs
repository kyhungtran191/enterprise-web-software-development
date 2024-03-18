using AutoMapper;
using Server.Application.Features.Authentication;
using Server.Contracts.Authentication;
using Server.Contracts.Authentication.Requests;

namespace Server.Api.Common.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LoginRequest, LoginQuery>();
        CreateMap<LoginResult, AuthenticationResponse>();
    }    
}