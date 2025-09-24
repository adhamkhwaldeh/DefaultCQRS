using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.UnitOfWork;
using AutoMapper;
using DefaultCQRS.Data;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using DefaultCQRS.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace DefaultCQRS.Handlers
{
    public class CreateCategoryHandler : EntityCreateCommandHandler<IUnitOfWork, Category, long, CreateCategoryDto, CategoryDto>
    {

        public CreateCategoryHandler(ILoggerFactory loggerFactory,
         IUnitOfWork dataContext,
         IMapper mapper,
         IHttpContextAccessor context,
         IDistributedCache cache,
         IServiceProvider provider)
         : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {
        }

        protected override async Task<Response<CategoryDto>> ProcessAsync(EntityCreateCommand<CreateCategoryDto, Response<CategoryDto>> request, CancellationToken cancellationToken)
        {
            //You can override or adjust the default implementation
            return await base.ProcessAsync(request, cancellationToken);
        }
    }
}