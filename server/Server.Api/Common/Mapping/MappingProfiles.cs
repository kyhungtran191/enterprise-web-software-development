using AutoMapper;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Dtos.Users;
using Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear;
using Server.Application.Features.AcademicYearApp.Commands.DeleteAcademicYear;
using Server.Application.Features.AcademicYearApp.Commands.UpdateAcademicYear;
using Server.Application.Features.AcademicYearApp.Queries.ActiveYear;
using Server.Application.Features.AcademicYearApp.Queries.GetAcademicYearById;
using Server.Application.Features.AcademicYearApp.Queries.GetAllAcademicYearPaging;
using Server.Application.Features.Authentication;
using Server.Application.Features.ContributionApp.Commands.ApproveContributions;
using Server.Application.Features.ContributionApp.Commands.CreateContribution;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Commands.RejectContribution;
using Server.Application.Features.ContributionApp.Commands.UpdateContribution;
using Server.Application.Features.ContributionApp.Queries.DownloadFile;
using Server.Application.Features.ContributionApp.Queries.GetActivityLog;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Application.Features.ContributionApp.Queries.GetRejectReason;
using Server.Application.Features.FacultyApp.Commands.CreateFaculty;
using Server.Application.Features.FacultyApp.Commands.DeleteFaculty;
using Server.Application.Features.FacultyApp.Commands.UpdateFaculty;
using Server.Application.Features.FacultyApp.Queries.GetAllFacutiesPaging;
using Server.Application.Features.FacultyApp.Queries.GetFacultyByName;
using Server.Application.Features.Identity.Tokens.Commands.RefreshToken;
using Server.Application.Features.Identity.Users.Commands.CreateUser;
using Server.Application.Features.Identity.Users.Commands.DeleteUserById;
using Server.Application.Features.Identity.Users.Commands.ForgotPassword;
using Server.Application.Features.Identity.Users.Commands.ResetPassword;
using Server.Application.Features.Identity.Users.Commands.UpdateProfile;
using Server.Application.Features.Identity.Users.Commands.UpdateUser;
using Server.Application.Features.Identity.Users.Queries.GetAllUsersPaging;
using Server.Application.Features.Identity.Users.Queries.GetProfile;
using Server.Application.Features.Identity.Users.Queries.GetUserById;
using Server.Application.Features.Identity.Users.Queries.ValidateForgotToken;
using Server.Application.Features.PublicContributionApp.Commands.CreateFavorite;
using Server.Application.Features.PublicContributionApp.Commands.CreateReadLater;
using Server.Application.Features.PublicContributionApp.Commands.LikeContribution;
using Server.Application.Features.PublicContributionApp.Commands.ViewContribution;
using Server.Application.Features.PublicContributionApp.Queries.DownAllFile;
using Server.Application.Features.PublicContributionApp.Queries.DownSingleFile;
using Server.Application.Features.PublicContributionApp.Queries.GetAllPublicContributionPaging;
using Server.Application.Features.PublicContributionApp.Queries.GetDetailPublicContributionBySlug;
using Server.Application.Features.TagApp.Commands.CreateTag;
using Server.Application.Features.TagApp.Commands.DeleteTag;
using Server.Application.Features.TagApp.Commands.UpdateTag;
using Server.Application.Features.TagApp.Queries.GetAllTagsPaging;
using Server.Application.Features.TagApp.Queries.GetTagById;
using Server.Contracts.AcademicYears;
using Server.Contracts.Authentication;
using Server.Contracts.Authentication.Requests;
using Server.Contracts.Contributions;
using Server.Contracts.Faculties;
using Server.Contracts.Identity.Tokens;
using Server.Contracts.Identity.Users;
using Server.Contracts.PublicContributions;
using Server.Contracts.PublicContributions.Favorite;
using Server.Contracts.PublicContributions.Like;
using Server.Contracts.PublicContributions.ReadLater;
using Server.Contracts.PublicContributions.View;
using Server.Contracts.Tags;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using File = Server.Domain.Entity.Content.File;

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

        // Refresh Token
        CreateMap<TokenRequest, RefreshTokenCommand>();        
        // User Profile
        CreateMap<GetProfileRequest, GetProfileQuery>();
        CreateMap<AppUser, UserProfileDto>();
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        CreateMap<UpdateProfileCommand, AppUser>();
        // Faculty
        CreateMap<Faculty, FacultyDto>();
        CreateMap<GetAllFacultiesPagingQuery, GetAllFacultiesPagingQuery>();
        CreateMap<GetFacultyByIdRequest, GetFacultyByIdQuery>();
        CreateMap<CreateFacultyRequest, CreateFacultyCommand>();
        CreateMap<UpdateFacultyRequest, UpdateFacultyCommand>();
        CreateMap<DeleteFacultyRequest, DeleteFacultyCommand>();
        CreateMap<GetAllFacultiesPagingRequest, GetAllFacultiesPagingQuery>();

        // Roles
        CreateMap<AppRole, RoleDto>();
        // Tags
        CreateMap<Tag, TagDto>();
        CreateMap<GetAllTagsPagingRequest, GetAllTagsPagingQuery>();
        CreateMap<GetTagByIdRequest, GetTagByIdQuery>();
        CreateMap<CreateTagRequest, CreateTagCommand>();
        CreateMap<UpdateTagRequest, UpdateTagCommand>();
        CreateMap<DeleteTagRequest, DeleteTagCommand>();
        // academic year
        CreateMap<AcademicYear, AcademicYearDto>();
        CreateMap<CreateAcademicYearCommand, AcademicYear>();

        CreateMap<CreateAcademicYearRequest, CreateAcademicYearCommand>();
        CreateMap<GetAllYearsPagingRequest, GetAllAcademicYearsPagingQuery>();
        CreateMap<GetYearByIdRequest, GetAcademicYearByIdQuery>();
        CreateMap<DeleteAcademicYearRequest, DeleteAcademicYearCommand>();
        CreateMap<UpdateAcademicYearRequest, UpdateAcademicYearCommand>();
        CreateMap<ActiveAcademicYearRequest, ActiveYearCommand>();

        // contributions
        CreateMap<Contribution, ContributionDto>();
        CreateMap<Contribution, ContributionInListDto>();
        CreateMap<Contribution, UpdateContributionCommand>().ReverseMap();
        CreateMap<GetContributionBySlugRequest, GetContributionBySlugQuery>();
        CreateMap<CreateContributionRequest, CreateContributionCommand>();
        CreateMap<GetAllContributionPagingRequest, GetAllContributionsPagingQuery>();
        CreateMap<UpdateContributionRequest, UpdateContributionCommand>();
        CreateMap<DeleteContributionRequest, DeleteContributionCommand>();
        CreateMap<ApproveContributionsRequest, ApproveContributionsCommand>();
        CreateMap<RejectContributionRequest, RejectContributionCommand>();
        CreateMap<GetRejectReasonRequest, GetRejectReasonQuery>();
        CreateMap<DownloadFileRequest, DownloadFileQuery>();
        // public contribution
        CreateMap<Contribution, ContributionPublic>();
        CreateMap<GetAllPublicContributionPagingRequest, GetAllPublicContributionPagingQuery>();
        CreateMap<LikeContributionRequest, LikeContributionCommand>();
        CreateMap<ViewContributionRequest, ViewContributionCommand>();
        CreateMap<ReadLaterRequest, CreateReadLaterCommand>();
        CreateMap<FavoriteRequest, CreateFavoriteCommand>();
        CreateMap<GetDetailPublicContributionBySlugRequest, GetDetailPublicContributionBySlugQuery>();
        CreateMap<DownSingleFileRequest, DownSingleFileQuery>();
        CreateMap<DownloadAllFileRequest, DownloadAllFileQuery>();
        // activity log
        CreateMap<ContributionActivityLog, ActivityLogDto>();
        CreateMap<GetActivityLogRequest, GetActivityLogQuery>();
        // file
        CreateMap<File,FileDto>();
        // for-got password
        CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>();
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
        CreateMap<ValidateForgotTokenRequest, ValidateForgotTokenQuery>();
    }    
}