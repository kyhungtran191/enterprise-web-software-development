using AutoMapper;
using Server.Application.Common.Dtos.Users;
using Server.Application.Features.Authentication;
using Server.Application.Features.Identity.Users.Commands.CreateUser;
using Server.Application.Features.Identity.Users.Commands.DeleteUserById;
using Server.Application.Features.Identity.Users.Commands.UpdateUser;
using Server.Application.Features.Identity.Users.Queries.GetAllUsersPaging;
using Server.Application.Features.Identity.Users.Queries.GetUserById;
using Server.Contracts.Authentication;
using Server.Contracts.Authentication.Requests;
using Server.Contracts.Identity.Users;
using Server.Domain.Entity.Identity;

namespace Server.Api.Common.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Login
        CreateMap<LoginRequest, LoginQuery>();
        CreateMap<LoginResult, AuthenticationResponse>();

        // Users
        CreateMap<AppUser, UserDto>().ReverseMap();
        
        // Create users
        CreateMap<CreateUserRequest, CreateUserCommand>();        
        CreateMap<CreateUserCommand, AppUser>();        

        // Update users
        CreateMap<UpdateUserRequest, UpdateUserCommand>();        
        CreateMap<UpdateUserCommand, AppUser>();        

        // Get user paging
        CreateMap<GetAllUserPagingRequest, GetAllUserPagingQuery>();

        // Get User by id
        CreateMap<GetUserByIdRequest, GetUserByIdQuery>();

        // Delete User By Id
        CreateMap<DeleteUserByIdRequest, DeleteUserByIdCommand>();
    }    
}