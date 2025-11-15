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
using Microsoft.AspNetCore.Mvc;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityCountQueryHandler<TUnitOfWork, TEntity>
        : DataContextHandlerBase<TUnitOfWork, EntityCountQuery<Response<int>>, Response<int>>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork
    {
        public EntityCountQueryHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<Response<int>> ProcessAsync(EntityCountQuery<Response<int>> request, CancellationToken cancellationToken)
        {
            var entityResponse = new Response<int>();
            try
            {
                var query = DataContext.Set<TEntity>().AsQueryable();
                if (request.Filter != null)
                {
                    query = query.Filter(request.Filter);
                }
                var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
                entityResponse.StatusCode = StatusCodes.Status200OK;
                entityResponse.ReturnStatus = true;
                entityResponse.Data = count;
                return entityResponse;
            }
            catch (Exception ex)
            {
                entityResponse.StatusCode = StatusCodes.Status404NotFound;
                entityResponse.ReturnMessage.Add(String.Format("Unable to Get Record from {0}" + 
                    ex.Message, typeof(TEntity).Name));
                entityResponse.ReturnStatus = false;
            }
            return entityResponse;
        }
    }
}
