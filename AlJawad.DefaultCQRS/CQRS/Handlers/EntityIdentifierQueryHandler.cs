using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityIdentifierQueryHandler<TUnitOfWork, TEntity, TKey, TReadModel>
       : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel,
         EntityIdentifierQuery<TKey, Response<TReadModel>>, Response<TReadModel>>
       where TEntity : class, IHaveIdentifier<TKey>, new()
       where TUnitOfWork : IUnitOfWork
       where TReadModel : class
    {
        [Obsolete]
        public EntityIdentifierQueryHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context,
            IServiceProvider provider,
            IDistributedCache cache)
            : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {
        }
        protected override async Task<Response<TReadModel>> ProcessAsync(EntityIdentifierQuery<TKey, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<TReadModel>();
            try
            {
                var model = await Read(request.Filter.Id, request.Filter.IncludeProperties, cancellationToken)
                    .ConfigureAwait(false);

                entityResponse.StatusCode = StatusCodes.Status200OK;
                entityResponse.ReturnStatus = true;
                entityResponse.Data = model;
            }
            catch (Exception ex)
            {
                entityResponse.StatusCode = StatusCodes.Status404NotFound;
                entityResponse.ReturnMessage.Add(String.Format("Unable to Get Record from {0} - with Id {1}" + ex.Message, typeof(TEntity).Name, request.Filter.Id.ToString()));
                entityResponse.ReturnStatus = false;
            }
            return entityResponse;
        }
    }
}
