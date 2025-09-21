using AlJawad.DefaultCQRS.Controllers;
using AlJawad.SqlDynamicLinker.Models;
using AutoMapper;
using MediatR;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Principal;
using AlJawad.DefaultCQRS.Models.Responses;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace DefaultCQRS.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(ExceptionDto))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductApiController : DefaultCommandControllerBase<long, Product, ProductDto, CreateProductDto, ProductDto, UpdateProductDto>
    {
        public ProductApiController(IMediator mediator, IDistributedCache appCache, IPrincipal httpContextAccessor, IAuthorizationService service, IMapper mapper) : base(mediator, appCache, httpContextAccessor, service, mapper)
        {
        }

        public override List<ColumnBase> IncludePropertiesForDetails => [];

        public override List<ColumnBase> IncludePropertiesForListing => [];

        public override List<ColumnBase> IncludePropertiesForManage => [];

        public override List<ColumnBase> SearchPropertiesForSearchByName => [];

    }
}
