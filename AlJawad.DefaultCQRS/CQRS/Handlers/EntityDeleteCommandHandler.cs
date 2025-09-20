using AlJawad.SqlDynamicLinker.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityDeleteCommandHandler<TUnitOfWork, TEntity, TKey, TReadModel>
       : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel, EntityDeleteCommand<TKey, Response<TReadModel>>, Response<TReadModel>>
       where TEntity : class, IHaveIdentifier<TKey>, new()
       where TUnitOfWork : IUnitOfWork
       where TReadModel : class
    {
        [Obsolete]
        public EntityDeleteCommandHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context,
            IDistributedCache cache,
            IServiceProvider provider)
            : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {
        }
        protected override async Task<Response<TReadModel>> ProcessAsync(EntityDeleteCommand<TKey, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<TReadModel>();
            try
            {
                var dbSet = DataContext.Set<TEntity>();
                var keyValue = new object[] { request.BaseFilter.Id };
                var entity = dbSet.Where(p => Equals(p.Id, request.BaseFilter.Id));


                if (request.BaseFilter.IncludeProperties != null && request.BaseFilter.IncludeProperties.Count() > 0)
                {
                    entity = entity.Includes(request.BaseFilter.IncludeProperties);
                    //entity = request.BaseFilter.IncludeProperties.Aggregate(entity, (current, includeProperty) =>
                    //        current.Include(includeProperty.DataName.Trim(new char[] { ' ', '\n', '\r' })));
                }
                var model = entity.FirstOrDefault();

                if (model == null)
                {
                    entityResponse.StatusCode = StatusCodes.Status404NotFound;
                    throw new Exception("Unable to delete Record., Record not found.");
                }
                dbSet.Remove(model).State = EntityState.Deleted;

                //dbSet.Remove(model);

                await DataContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                DataContext.CommitTransaction();
                entityResponse.StatusCode = StatusCodes.Status200OK;
                entityResponse.ReturnStatus = true;

                if (typeof(TEntity).Implements<ICache>())
                    Cache.Remove(typeof(TReadModel).Name);

                entityResponse.Data = default;
            }
            catch (Exception ex)
            {
                entityResponse.StatusCode = StatusCodes.Status400BadRequest;
                entityResponse.ReturnMessage.Add(string.Format("Unable to delete Record {0}" + ex.Message, typeof(TEntity).Name));
                entityResponse.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return entityResponse;
        }

    }
}
