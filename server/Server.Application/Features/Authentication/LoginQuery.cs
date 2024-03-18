using AutoMapper;
using ErrorOr;
using MediatR;

namespace Server.Application.Features.Authentication;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<LoginResult>>;


