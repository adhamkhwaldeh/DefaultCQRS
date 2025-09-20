using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AlJawad.DefaultCQRS.CQRS.Commands;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.DefaultCQRS.Extensions;
using static System.Net.WebRequestMethods;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.CQRS.Commands;

namespace AlJawad.DefaultCQRS.Controllers
{
    //[Authorize]
    public abstract class CommandControllerBase<TKey,TDtoModel, TCreateModel, TReadModel, TUpdateModel>
        : QueryControllerBase<TKey, TDtoModel, TReadModel>
    {

        protected CommandControllerBase(IMediator mediator, IDistributedCache cache, IPrincipal httpContext,
            IAuthorizationService service) : base(mediator, cache, httpContext, service)
        {

        }

        #region Include and search Properties  
        public abstract List<ColumnBase> IncludePropertiesForDetails { get; }

        public abstract List<ColumnBase> IncludePropertiesForListing { get; }

        public abstract List<ColumnBase> IncludePropertiesForManage { get; }

        public abstract List<ColumnBase> SearchPropertiesForSearchByName { get; }

        protected List<EntityFilter> BuildSearchFilter(string term)
        {
            List<EntityFilter> DynamicFilters = new List<EntityFilter>();
            if (string.IsNullOrWhiteSpace(term))
            {
                return DynamicFilters;
            }
            foreach (var column in SearchPropertiesForSearchByName)
            {
                DynamicFilters.Add(new EntityFilter()
                {
                    DataName = column.DataName,
                    Value = term,
                    Operator = EntityFilterOperators.Contains,
                    Logic = EntityFilterLogic.Or,
                });
            }
            return DynamicFilters;
        }

        protected IEnumerable<EntityBaseFilter> MergeSearchAndDefaultFilter(IEnumerable<EntityBaseFilter> DynamicFilters, String? SearchQuery)
        {
            if (SearchQuery == null && SearchQuery.NullOrEmpty())
            {
                return DynamicFilters;
            }
            var SearchFilters = BuildSearchFilter(SearchQuery);
            if (SearchFilters.nullOrEmpty())
            {
                return DynamicFilters;
            }
            if (DynamicFilters.nullOrEmpty())
            {
                return SearchFilters;
            }

            return new List<EntityFilter>()
            {
                new EntityFilter(){
                Filters = DynamicFilters
            },
                new EntityFilter(){
                Logic = EntityFilterLogic.And,
                Filters = SearchFilters
            }
            };

        }
        #endregion

        protected virtual async Task<ResponseArray<TReadModel>> BulkCreate(IEnumerable<TCreateModel> createModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityBulkCreateCommand<TCreateModel, ResponseArray<TReadModel>>(Context, createModel);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<ResponseArray<TReadModel>> BulkUpdate(IEnumerable<TUpdateModel> updateModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityBulkCreateCommand<TUpdateModel, ResponseArray<TReadModel>>(Context, updateModel);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<Models.Responses.Response<TReadModel>> Create(BaseFilter baseFilter, TCreateModel createModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityCreateCommand<TCreateModel, Models.Responses.Response<TReadModel>>(Context, baseFilter, createModel);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected virtual async Task<Models.Responses.Response<TReadModel>> Update(BaseIdentifierFilter<TKey> baseFilter , TUpdateModel updateModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityUpdateCommand<TKey, TUpdateModel, Models.Responses.Response<TReadModel>>(Context, baseFilter, updateModel);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected virtual async Task<Models.Responses.Response<TReadModel>> Delete(BaseIdentifierFilter<TKey> baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityDeleteCommand<TKey, Models.Responses.Response<TReadModel>>(Context, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<Models.Responses.Response<TReadModel>> BulkDelete(BaseIdentifierFilter<TKey> baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityDeleteCommand<TKey, Models.Responses.Response<TReadModel>>(Context, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<Models.Responses.Response<TReadModel>> Disable(BaseIdentifierFilter<TKey> baseFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = new EntityDisabledCommand<TKey, Models.Responses.Response<TReadModel>>(Context, baseFilter);
            var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
      
        protected virtual void AddHeaders(PublishContext context)
        {
            var item = Request.Headers["Authorization"].ToString().Replace("bearer ", string.Empty).Replace("Bearer ", string.Empty);
            context.Headers.Set("Authorization", item);
        }
        
        protected virtual Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".zip", "application/zip"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4", "video/mp4" }
            };
        }
      
    }
}