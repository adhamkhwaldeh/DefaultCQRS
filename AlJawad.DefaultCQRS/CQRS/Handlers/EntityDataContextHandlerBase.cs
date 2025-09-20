using AutoMapper;
using AutoMapper.QueryableExtensions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Caching;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public abstract class EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel, TRequest, TResponse>
         : DataContextHandlerBase<TUnitOfWork, TRequest, TResponse>
         where TUnitOfWork : IUnitOfWork
         where TEntity : class, IHaveIdentifier<TKey>, new()
         where TRequest : IRequest<TResponse>
         where TReadModel : class

    {
       // private static ConfigurationCode Configuration { get; set; }
        protected readonly IDistributedCache Cache;
        protected readonly IServiceProvider Provider;
        protected readonly IHttpContextAccessor Context;

        protected EntityDataContextHandlerBase(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context,
            IDistributedCache cache,
            IServiceProvider provider

           )
            : base(loggerFactory, dataContext, mapper)
        {
            Assert.NotNull(cache, nameof(cache));
            Assert.NotNull(context, nameof(context));
            Assert.NotNull(provider, nameof(provider));
            Cache = cache;
            Context = context;
            Provider = provider;

        }

        protected virtual void AddHeaders(PublishContext context)
        {

            var item = Context.HttpContext.Request.Headers["Authorization"].ToString().Replace("bearer ", string.Empty).Replace("Bearer ", string.Empty);
            context.Headers.Set("Authorization", item);
        }
        protected virtual async Task<IEnumerable<TReadModel>> GetCache(string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task<IEnumerable<TReadModel>> ReadModel() => ListQuery(cancellationToken);
            return await Cache.GetAndCacheList(key, ReadModel, 20);
        }

        private async Task<IEnumerable<TReadModel>> ListQuery(CancellationToken cancellationToken)
        {
            var model = await DataContext
                .Set<TEntity>()
                .AsNoTracking()
                .ProjectTo<TReadModel>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return model.AsEnumerable();
        }

        protected virtual async Task<TReadModel> Read(TKey key , IEnumerable<ColumnBase> propertyList, CancellationToken cancellationToken = default(CancellationToken))
        {
            var model = DataContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(p => Equals(p.Id, key));

            if (propertyList!=null)
            {
                model = propertyList.Aggregate(model, (current, s) => current.Include(s.DataName.Trim(new char[] { ' ', '\n', '\r' })));
            }

            var result = await model.FirstOrDefaultAsync().ConfigureAwait(false);
            return Mapper.Map<TReadModel>(result);
        }

        //protected virtual async Task<TReadModel> Read(TKey key, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var model = await DataContext
        //        .Set<TEntity>()
        //        .AsNoTracking()
        //        .Where(p => Equals(p.Id, key))
        //        .FirstOrDefaultAsync(cancellationToken)
        //        .ConfigureAwait(false);
        //    return Mapper.Map<TReadModel>(model);
        //}

        //protected virtual TEntity GenerateCode(TEntity entity, short CodeType, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var dbSetCode = DataContext.Set<ConfigurationCode>();
        //    if (CodeType == 0) throw new RectaDomainException(System.Net.HttpStatusCode.NotFound, "Code configuration is required.,");
        //    Configuration = dbSetCode.GetByCode(CodeType);
        //    entity.GetType().GetProperty("Code")?.SetValue(entity, Configuration.Code);
        //    dbSetCode.Update(Configuration);
        //    return entity;
        //}


    }
}
