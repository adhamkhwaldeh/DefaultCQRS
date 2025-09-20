using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityUpsertCommandHandler<TUnitOfWork, TEntity, TKey, TModel, TReadModel>
        : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel, EntityUpsertCommand<TKey, TModel, Response<TReadModel>>, Response<TReadModel>>
        where TUnitOfWork : IUnitOfWork
        where TEntity : class, IHaveIdentifier<TKey>, new()
        where TReadModel : class
    {
        public EntityUpsertCommandHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
                                          IMapper mapper,
                                          IHttpContextAccessor context,
                                          IServiceProvider provider,
                                          IDistributedCache cache
        ) : base(loggerFactory, dataContext, mapper, context, cache, provider)

        {

        }


        protected override async Task<Response<TReadModel>> ProcessAsync(EntityUpsertCommand<TKey, TModel, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<TReadModel>();
            try
            {
                var dbSet = DataContext
                    .Set<TEntity>();

                var keyValue = new object[] { request.Id };

                // find entity to update by message id, not model id
                var entity = await dbSet
                    .FindAsync(keyValue, cancellationToken)
                    .ConfigureAwait(false);

                // create entity if not found
                if (entity == null)
                {
                    entity = new TEntity { Id = request.Id };

                    await dbSet
                        .AddAsync(entity, cancellationToken)
                        .ConfigureAwait(false);
                }

                // copy updates from model to entity
                Mapper.Map(request.Model, entity);
                // save updates
                await DataContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                // return read model
                //TODO need to include the filter and include properties
                var readModel = await Read(entity.Id,new List<ColumnBase>(), cancellationToken)
                    .ConfigureAwait(false);
                entityResponse.ReturnStatus = true;
                entityResponse.Data = readModel;
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