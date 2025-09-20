using AlJawad.SqlDynamicLinker.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.Caching;
using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.Entities;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityLocationsListQueryHandler<TUnitOfWork, TEntity, TKey>
        : DataContextHandlerBase<TUnitOfWork, EntityLocationsListQuery<TEntity, ResponseArray<EntityLocationModel<TKey>>>, ResponseArray<EntityLocationModel<TKey>>>
        where TEntity : class, IPointEntity, IHaveIdentifier<TKey>
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IDistributedCache Cache;
        public EntityLocationsListQueryHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper, IDistributedCache cache)
            : base(loggerFactory, dataContext, mapper)
        {
            Cache = cache;
        }

        protected override async Task<ResponseArray<EntityLocationModel<TKey>>> ProcessAsync(EntityLocationsListQuery<TEntity, ResponseArray<EntityLocationModel<TKey>>> request, CancellationToken cancellationToken)
        {

            var entityResponse = new ResponseArray<EntityLocationModel<TKey>>();
            try
            {
                var query = DataContext.Set<TEntity>().AsQueryable();

                query = BuildQuery(request, query);
                var result = await EfQueryList(request, query, cancellationToken)
                                                            .ConfigureAwait(false);
                //TODO need to be checked
                //if (typeof(TEntity).Implements<ICache>())
                //    await Cache.GetAndCacheList(typeof(TReadModel).Name, () => Task.FromResult(result.Data.AsEnumerable()), 20).ConfigureAwait(false);

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
        protected virtual IQueryable<TEntity> BuildQuery(EntityLocationsListQuery<TEntity,ResponseArray<EntityLocationModel<TKey>>> request, IQueryable<TEntity> query)
        {
            if (request?.Filter != null)
                query = query.Filter(request.Filter);
            return query;
        }

        protected virtual async Task<ResponseArray<EntityLocationModel<TKey>>> EfQueryList(EntityLocationsListQuery<TEntity,ResponseArray<EntityLocationModel<TKey>>> request, IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            var result = new ResponseArray<EntityLocationModel<TKey>>();
            try
            {

                var items = query.Any() ? await query.Sort(request.Filter.DynamicSorting)
                    .Select(e=> new EntityLocationModel<TKey>()
                    {
                        Id = e.Id,
                        Location = Mapper.Map<PointModel>(e.Location)
                    }).ToListAsync().ConfigureAwait(false) : new List<EntityLocationModel<TKey>>();

                result.Data = Mapper.Map<List<EntityLocationModel<TKey>>>(items);

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

        protected virtual async Task<IReadOnlyCollection<EntityLocationModel<TKey>>> CacheQueryList(EntityLocationsListQuery<TEntity,EntityLocationModel<TKey>> request, IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            var map = Mapper.Map<IReadOnlyCollection<EntityLocationModel<TKey>>>(query.Sort(request.Filter.DynamicSorting).ToList());
            return await Task.FromResult(map).ConfigureAwait(false);
        }

    }
}