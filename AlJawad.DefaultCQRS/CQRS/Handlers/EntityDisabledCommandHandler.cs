using AlJawad.SqlDynamicLinker.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityDisabledCommandHandler<TUnitOfWork, TEntity, TKey, TReadModel>
       : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel, EntityDisabledCommand<TKey, Response<TReadModel>>, Response<TReadModel>>
       where TEntity : class, IHaveIdentifier<TKey>, new()
       where TUnitOfWork : IUnitOfWork
       where TReadModel : class
    {
        [Obsolete]
        public EntityDisabledCommandHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context,
            IServiceProvider provider,
            IDistributedCache cache)
            : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {
        }
        protected override async Task<Response<TReadModel>> ProcessAsync(EntityDisabledCommand<TKey, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<TReadModel>();
            try
            {

                var dbSet = DataContext
                .Set<TEntity>();
            
                var entity = dbSet.Where(p => Equals(p.Id, request.BaseFilter.Id));
                if (request.BaseFilter.IncludeProperties != null && request.BaseFilter.IncludeProperties.Count() > 0)
                {
                    entity = entity.Includes(request.BaseFilter.IncludeProperties);
                    //entity = request.BaseFilter.IncludeProperties.Aggregate(entity, (current, s) => current.Include(s.DataName.Trim(new char[] { ' ', '\n', '\r' })));
                }
                var model = entity.FirstOrDefault();
                if (model == null)
                    return default;

                dbSet.Remove(model).State = EntityState.Deleted;
                await DataContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                var result = Mapper.Map<TReadModel>(model);
                entityResponse.ReturnStatus = true;
                entityResponse.Data = result;
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
