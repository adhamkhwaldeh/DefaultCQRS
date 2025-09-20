using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Extensions;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityPagedQueryHandler<TUnitOfWork, TEntity, TReadModel>
       : DataContextHandlerBase<TUnitOfWork, EntityPagedQuery<ResponseList<TReadModel>>, ResponseList<TReadModel>>
       where TEntity : class
       where TUnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;

        public EntityPagedQueryHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
            _mapper = mapper;
        }

        protected override async Task<ResponseList<TReadModel>> ProcessAsync(EntityPagedQuery<ResponseList<TReadModel>> request, CancellationToken cancellationToken)
        {
            var model = DataContext.Set<TEntity>().AsQueryable();

            if (request.Filter.IncludeProperties != null)
            {
                model = model.Includes(request.Filter.IncludeProperties);
                //  request.Filter.IncludeProperties.Aggregate(model, (current, includeProperty) => current.Include(includeProperty.DataName));
            }
            // build query from filter
            model = BuildQuery(request, model);

            //get total for query


            var total = await QueryTotal(model, cancellationToken)
                .ConfigureAwait(false);

            //short circuit if total is zero
            if (total == 0)
            {
                return new ResponseList<TReadModel>
                {
                    Data = new List<TReadModel>(),
                    StatusCode = StatusCodes.Status200OK,
                    ReturnMessage = new List<string>() { "Record Not Found" },
                };
            }
            var data = model.Sort(request.Filter.DynamicSorting)
               .Page(request.Filter.Page, request.Filter.PageSize).ToList();

            // page the query and convert to read model
            var result = _mapper.Map<List<TReadModel>>(data);


            return new ResponseList<TReadModel>
            {
                Total = total,
                PageCount = (request.Filter.Page - 1) * request.Filter.PageSize + result.Count,
                Data = result,
                Page = request.Filter.Page,
                PageSize = request.Filter.PageSize,
                StatusCode = StatusCodes.Status200OK,
                ReturnStatus = true,
            };
        }

        private static async Task<int> QueryTotal(IQueryable<TEntity> model, CancellationToken cancellationToken)
        {
            return await model
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private IQueryable<TEntity> BuildQuery(EntityPagedQuery<ResponseList<TReadModel>> request, IQueryable<TEntity> model)
        {
            var entityQuery = request;

            //build query from filter
            if (entityQuery?.Filter != null)
                model = model.Filter(entityQuery.Filter);

            //add raw query
            //TODO need to be reviewed
            //if (entityQuery != null && !string.IsNullOrEmpty(entityQuery.Filter.Query))
            //    model = model.Where(entityQuery.Query);

            return model;
        }
    }
}
