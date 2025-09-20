using AlJawad.DefaultCQRS.Controllers;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.CQRS.Queries;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Principal;
using AlJawad.SqlDynamicLinker.Models;
//using MassTransit.Mediator;

namespace DefaultCQRS.Controllers
{
    public class ProductController : DefaultCommandControllerBase<long, Product, ProductDto, CreateProductDto, ProductDto, UpdateProductDto>
    {
        public ProductController(IMediator mediator, IDistributedCache appCache, IPrincipal httpContextAccessor, IAuthorizationService service, IMapper mapper) : base(mediator, appCache, httpContextAccessor, service, mapper)
        {
        }

        public override List<ColumnBase> IncludePropertiesForDetails => [];

        public override List<ColumnBase> IncludePropertiesForListing => [];

        public override List<ColumnBase> IncludePropertiesForManage => [];

        public override List<ColumnBase> SearchPropertiesForSearchByName => [];

    }
}
