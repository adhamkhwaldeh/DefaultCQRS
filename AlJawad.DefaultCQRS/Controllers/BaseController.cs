//using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.IO;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AlJawad.DefaultCQRS.Controllers
{
    //[Produces("application/json")]
    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
    //[ProducesResponseType(500, Type = typeof(ExceptionDto))]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    //public abstract class BaseController : ControllerBase
     public abstract class BaseController :  Controller
     {
        public IDistributedCache AppCache { get; set; }
        public IMediator Mediator { get; }
        public IPrincipal Context { get; }
        public IAuthorizationService AuthorizationService;
        public string DirectoryPath { get; set; } = Path.Combine("Resources", "V1");

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
        protected BaseController(IMediator mediator,
            IDistributedCache cache,
            IPrincipal principal,
            IAuthorizationService authorizationService)
        {
            Mediator = mediator;
            AppCache = cache;
            Context = principal;
            AuthorizationService = authorizationService;
        }

    }

}