
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.Extensions;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{

    public class EntityUpdateCommandHandler<TUnitOfWork, TEntity, TKey, TModel, TReadModel>
         : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel,
           EntityUpdateCommand<TKey, TModel, Response<TReadModel>>, Response<TReadModel>>
         where TUnitOfWork : IUnitOfWork
         where TEntity : class, IHaveIdentifier<TKey>, new()
         where TReadModel : class
    {
        public EntityUpdateCommandHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context
            , IDistributedCache cache,
            IServiceProvider provider)
            : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {

        }

        protected override async Task<Response<TReadModel>> ProcessAsync(EntityUpdateCommand<TKey, TModel, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<TReadModel>();
            try
            {
                var dbSet = DataContext.Set<TEntity>();
                var keyValue = new object[] { request.BaseFilter.Id };
                var entity = await dbSet.FindAsync(keyValue, cancellationToken).ConfigureAwait(false);

                if (entity == null) return default;
                Mapper.Map(request.Model, entity);

                dbSet.Update(entity);
                //dbSet.Update(entity, cancellationToken).ConfigureAwait(false);

                await DataContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
                DataContext.CommitTransaction();
                entityResponse.StatusCode = StatusCodes.Status200OK;
                entityResponse.ReturnStatus = true;

                entityResponse.Data = await Read(entity.Id, request.BaseFilter.IncludeProperties, cancellationToken);

                if (typeof(TEntity).Implements<ICache>()) Cache.Remove(typeof(TReadModel).Name);
            }
            catch (Exception ex)
            {
                entityResponse.ReturnMessage.Add(string.Format("Unable to Update Record {0}" + ex.Message, typeof(TEntity).Name));
                entityResponse.ReturnStatus = false;
            }
            return entityResponse;
        }
    }
}