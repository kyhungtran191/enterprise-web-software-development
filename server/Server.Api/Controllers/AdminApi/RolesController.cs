using System.Reflection;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos;
using Server.Application.Common.Extensions;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;
using Server.Contracts.Identity.Roles;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Api.Controllers.AdminApi;

public class RolesController : AdminApiController
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;

    public RolesController(ISender mediatorSender,
                          IMapper mapper,
                          RoleManager<AppRole> roleManager) : base(mediatorSender)
    {
        _mapper = mapper;
        _roleManager = roleManager;
    }

    private IActionResult ProblemWithError(Error error)
        => Problem(new List<Error> { error });

    [HttpPost]
    [Authorize(Permissions.Roles.Create)]
    public async Task<IActionResult> CreateRole([FromBody] CreateUpdateRoleRequest createUpdateRoleRequest)
    {
        var roleExists = await _roleManager.FindByNameAsync(createUpdateRoleRequest.Name);

        if (roleExists is not null)
        {
            return ProblemWithError(Errors.Roles.NameDuplicated);
        }

        var result = await _roleManager.CreateAsync(new AppRole
        {
            DisplayName = createUpdateRoleRequest.DisplayName,
            Name = createUpdateRoleRequest.Name,
        });

        return result.Succeeded ? Ok(ResponseWrapper.Success()) : Problem(result.GetIdentityResultErrorDescriptions());
    }

    [HttpPut("{id}")]
    [Authorize(Permissions.Roles.Edit)]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] CreateUpdateRoleRequest createUpdateRoleRequest)
    {
        if (string.IsNullOrEmpty(id))
        {
            return ProblemWithError(Errors.Roles.EmptyId);
        }

        var roleInDb = await _roleManager.FindByIdAsync(id);

        if (roleInDb is null)
        {
            return ProblemWithError(Errors.Roles.NotFound);
        }

        roleInDb.DisplayName = createUpdateRoleRequest.DisplayName;
        roleInDb.Name = createUpdateRoleRequest.Name;
        var result = await _roleManager.UpdateAsync(roleInDb);

        return result.Succeeded ? Ok(ResponseWrapper.Success(message: "Update roles successfully!")) : Problem(result.GetIdentityResultErrorDescriptions());
    }

    [HttpDelete]
    [Authorize(Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteRoles([FromQuery] Guid[] roleIds)
    {
        foreach (var roleId in roleIds)
        {
            var roleInDb = await _roleManager.FindByIdAsync(roleId.ToString());

            if (roleInDb is null)
            {
                return ProblemWithError(Errors.Roles.NotFound);
            }

            var resultAfterDeleting = await _roleManager.DeleteAsync(roleInDb);

            if (resultAfterDeleting.Succeeded == false)
            {
                return ProblemWithError(Errors.Roles.DeleteFailed);
            }
        }

        return Ok(ResponseWrapper.Success(message: "Delete Role(s) Successfully!"));
    }

    [HttpGet("{id}")]
    [Authorize(Permissions.Roles.View)]
    public async Task<IActionResult> GetRoleById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return ProblemWithError(Errors.Roles.EmptyId);
        }

        var roleInDb = await _roleManager.FindByIdAsync(id);

        if (roleInDb is null)
        {
            return ProblemWithError(Errors.Roles.NotFound);
        }

        var mappedRole = _mapper.Map<RoleDto>(roleInDb);

        return Ok(ResponseWrapper<RoleDto>.Success(mappedRole));
    }

    [HttpGet]
    [Authorize(Permissions.Roles.View)]
    public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
    {
        var model = await _mapper.ProjectTo<RoleDto>(_roleManager.Roles).ToListAsync();

        return Ok(ResponseWrapper<List<RoleDto>>.Success(model));
    }

    [HttpGet]
    [Route("paging")]
    [Authorize(Permissions.Roles.View)]
    public async Task<ActionResult<PagedResult<RoleDto>>> GetAllRolesPaging(string? keyword,
                                                                            int pageIndex = 1,
                                                                            int pageSize = 10)
    {
        var query = _roleManager.Roles;

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(role => role.Name!.Contains(keyword) || role.DisplayName.Contains(keyword));
        }

        var totalRow = await query.CountAsync();

        var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageSize;

        query =
            query
            .Skip(skipRow)
            .Take(pageSize);

        var data = await _mapper.ProjectTo<RoleDto>(query).ToListAsync();

        var response = new PagedResult<RoleDto>
        {
            CurrentPage = pageIndex,
            PageSize = pageSize,
            RowCount = totalRow,
            Results = data
        };

        return Ok(ResponseWrapper<PagedResult<RoleDto>>.Success(response));
    }


    [HttpGet("{id}/permissions")]
    [Authorize(Permissions.Roles.View)]
    public async Task<IActionResult> GetAllRolePermissions(string id)
    {
        var allPermissions = new List<RoleClaimsDto>();

        var permissionsNestedType = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;

        foreach (var permissionType in permissionsNestedType)
        {
            allPermissions.GetPermissionsByType(permissionType);
        }

        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
        {
            return ProblemWithError(Errors.Roles.NotFound);
        }

        var curentRoleClaims = await _roleManager.GetClaimsAsync(role);

        var currentRoleClaimValues = curentRoleClaims.Select(crl => crl.Value);
        var allPermissionValues = allPermissions.Select(x => x.Value);

        var authorizedClaims = currentRoleClaimValues.Intersect(allPermissionValues).ToHashSet();

        // check selected role
        foreach (var permission in allPermissions)
        {
            if (authorizedClaims.Contains(permission.Value))
            {
                permission.Selected = true;
            }
        }

        return Ok(ResponseWrapper<PermissionDto>.Success(new PermissionDto
        {
            RoleId = role.Id.ToString(),
            RoleClaims = allPermissions,
        }));
    }

    [HttpPut]
    [Authorize(Permissions.Roles.Edit)]
    public async Task<IActionResult> SavePermissions([FromBody] PermissionDto permission)
    {
        var role = await _roleManager.FindByIdAsync(permission.RoleId);

        if (role is null)
        {
            return ProblemWithError(Errors.Roles.NotFound);
        }

        var userPermissionsModified =
            permission
            .RoleClaims
            .Where(rl => rl.Selected)
            .Select(rl => rl.Value)
            .ToList();

        var currentlyUserPermissions = await _roleManager.GetClaimsAsync(role);

        foreach (var userPermission in currentlyUserPermissions)
        {
            await _roleManager.RemoveClaimAsync(role, userPermission);
        }

        foreach (var userPermissionModified in userPermissionsModified)
        {
            await _roleManager.AddPermissionClaim(role, userPermissionModified!);
        }

        return Ok(ResponseWrapper.Success(message: "Save perrmissions successfully!"));
    }
}