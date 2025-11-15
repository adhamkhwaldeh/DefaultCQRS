using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AlJawad.DefaultCQRS.CQRS.Queries;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AlJawad.DefaultCQRS.Controllers
{

    public abstract class QueryControllerBase<TKey, TDtoModel, TReadModel> : BaseController
    {
        protected QueryControllerBase(IMediator mediator, IDistributedCache cache, IPrincipal httpContextAccessor, IAuthorizationService service)
            : base(mediator, cache, httpContextAccessor, service)
        {

        }

        protected virtual async Task<Response<TReadModel>> Find(BaseIdentifierFilter<TKey> baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityIdentifierQuery<TKey, Response<TReadModel>>(Context, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected virtual async Task<ActionResult<int>> GetCount(BaseQueryableFilter baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new GetCountQuery<ActionResult<int>>(User, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }
     
        protected virtual async Task<ResponseList<TReadModel>> PagedQuery(BasePagingFilter baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new EntityPagedQuery<ResponseList<TReadModel>>(User, baseFilter);
            var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected virtual async Task<ResponseArray<TReadModel>> ListQuery(BaseQueryableFilter baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityListQuery<ResponseArray<TReadModel>>(User, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected virtual async Task<ResponseArray<TReadModel>> ListQuery(CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityListQuery<ResponseArray<TReadModel>>(User);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }

        //TODO need to be segregated to enhance the performance
        protected virtual async Task<ResponseArray<TReadModel>> Max(BaseQueryableFilter baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityListQuery<ResponseArray<TReadModel>>(User, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }


        //protected virtual async Task<Response<TReadModel>> Find(TKey id, string properties, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var command = new EntityIdentifierQuery<TKey, Response<TReadModel>>(Context, id, properties);
        //    var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        //    return result;
        //}


        //protected virtual async Task<ResponseArray<TReadModel>> ListQuery(string properties, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var command = new EntityListQuery<ResponseArray<TReadModel>>(User, BaseFilter(properties));
        //    var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        //    return result;
        //}

        //protected virtual async Task<ResponseArray<TReadModel>> ListQuery(string properties, EntityFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var command = new EntityListQuery<ResponseArray<TReadModel>>(User, filter, properties);
        //    var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        //    return result;
        //}

        //protected virtual async Task<ResponseArray<TReadModel>> ListQuery(string properties, EntityFilter filter, List<EntitySort> sort, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var command = new EntityListQuery<ResponseArray<TReadModel>>(User, filter, sort, properties);
        //    var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        //    return result;
        //}



    }
}