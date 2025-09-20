using AutoMapper;
using MassTransit.Caching;
using MassTransit.Internals.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS;
using AlJawad.DefaultCQRS.Caching;
using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Extensions;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityListQueryHandler<TUnitOfWork, TEntity, TReadModel>
        : DataContextHandlerBase<TUnitOfWork, EntityListQuery<ResponseArray<TReadModel>>, ResponseArray<TReadModel>>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork
        where TReadModel : class
    {
        private readonly IDistributedCache Cache;
        public EntityListQueryHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper, IDistributedCache cache)
            : base(loggerFactory, dataContext, mapper)
        {
            Cache = cache;
        }

        protected override async Task<ResponseArray<TReadModel>> ProcessAsync(EntityListQuery<ResponseArray<TReadModel>> request, CancellationToken cancellationToken)
        {
            
            var entityResponse = new ResponseArray<TReadModel>();
            try
            {
                var query = DataContext.Set<TEntity>().AsQueryable();
                if (request.Filter.IncludeProperties != null && request.Filter.IncludeProperties.Count() > 0)
                {
                    query = query.Includes(request.Filter.IncludeProperties);
                    //query = request.Filter.IncludeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty.DataName));
                }
             
                query = BuildQuery(request, query);
                var result = await EfQueryList(request, query, cancellationToken)
                                                            .ConfigureAwait(false);

                if (typeof(TEntity).Implements<ICache>())
                    await Cache.GetAndCacheList(typeof(TReadModel).Name, () => Task.FromResult(result.Data.AsEnumerable()), 20).ConfigureAwait(false);

                result.StatusCode = StatusCodes.Status200OK;
                result.ReturnStatus = true;

                return result;
            }
            catch (Exception ex)
            {
                entityResponse.ReturnMessage.Add("Record not found");
                entityResponse.Status = false;
            }
            return entityResponse;
        }
        protected virtual IQueryable<TEntity> BuildQuery(EntityListQuery<ResponseArray<TReadModel>> request, IQueryable<TEntity> query)
        {
            if (request?.Filter != null)
                query = query.Filter(request.Filter);
            return query;
        }

        protected virtual async Task<ResponseArray<TReadModel>> EfQueryList(EntityListQuery<ResponseArray<TReadModel>> request, IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            var result = new ResponseArray<TReadModel>();
            try
            {

                var items = query.Any() ? await query.Sort(request.Filter.DynamicSorting).ToListAsync().ConfigureAwait(false) : new List<TEntity>();
                result.Data = Mapper.Map<List<TReadModel>>(items);

                result.Status = true;

                if (!query.Any())
                    result.ReturnMessage = new List<string>() { "Record not found." };
            }
            catch (Exception ex)
            {
                result.Errors.Add("GetAll", ex.Message);
                result.ReturnMessage.Add("Record not found");
                result.Status = false;

            }
            return result;
        }

        protected virtual async Task<IReadOnlyCollection<TReadModel>> CacheQueryList(EntityListQuery<TReadModel> request, IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            var map = Mapper.Map<IReadOnlyCollection<TReadModel>>(query.Sort(request.Filter.DynamicSorting).ToList());
            return await Task.FromResult(map).ConfigureAwait(false);
        }

    }
}