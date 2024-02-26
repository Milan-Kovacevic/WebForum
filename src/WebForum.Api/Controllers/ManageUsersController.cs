using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]/{userId:guid}")]
[AllowAnonymous]
public class ManageUsersController(ISender sender) : ApiController(sender)
{
    [HttpPut("Groups")]
    public async Task<IActionResult> ChangeUserGroup([FromRoute] Guid userId)
    {
        await Task.CompletedTask;
        return Ok(userId);
    }
    
    [HttpPost("Permissions/Add")]
    public async Task<IActionResult> AddUserPermission([FromRoute] Guid userId)
    {
        await Task.CompletedTask;
        return Ok(userId);
    }
    
    [HttpPost("Permissions/Remove")]
    public async Task<IActionResult> RemoveUserPermission([FromRoute] Guid userId)
    {
        await Task.CompletedTask;
        return Ok(userId);
    }
}