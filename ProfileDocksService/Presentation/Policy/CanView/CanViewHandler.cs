using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProfileDocksService.Presentation.Policy.CanView;

public class CanViewHandler : AuthorizationHandler<CanViewRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanViewRequirement requirement)
    {
        var userId = Guid.Parse(
            context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var role = context.User.FindFirst(ClaimTypes.Role)!.Value;

        var httpContext = context.Resource as HttpContext;
        var studentIdStr = httpContext?.Request.RouteValues["userId"]?.ToString();

        if (studentIdStr == null)
            return;
        
        var studentId = Guid.Parse(studentIdStr);
        if (userId == studentId)
        {
            context.Succeed(requirement);
            return;
        }
        
        if (role != "Applicant")
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail();
    }
}