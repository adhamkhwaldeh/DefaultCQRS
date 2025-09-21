using AlJawad.DefaultCQRS.CQRS.Permissions;
using DefaultCQRS.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DefaultCQRS.Authorization
{
    public class ProductAuthorizationHandler : AuthorizationHandler<BaseRequirement<Product, int>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BaseRequirement<Product, int> requirement)
        {
            // For demonstration purposes, we'll just succeed.
            // In a real application, you would implement your authorization logic here.
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
