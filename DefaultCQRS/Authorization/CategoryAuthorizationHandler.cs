using AlJawad.DefaultCQRS.Authorization;
using DefaultCQRS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DefaultCQRS.Authorization
{
    public class CategoryAuthorizationHandler : DefaultAuthorizationHandler<Category>
    {
        public CategoryAuthorizationHandler(IAuthorizationService authorizationService) : base(authorizationService)
        {
        }
    }
}