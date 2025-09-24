using AlJawad.DefaultCQRS.CQRS.Permissions;
using DefaultCQRS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DefaultCQRS.Authorization
{
    public class CategoryAuthorizationHandler : AuthorizationHandler<BaseRequirement<Category, long>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BaseRequirement<Category, long> requirement)
        {
            // For demonstration purposes, we'll just succeed.
            // In a real application, you would implement your authorization logic here.
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}