using AutoMapper;
using DataTables.AspNetCore.Mvc.Binder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinker.Models;
using System.Security.Principal;
using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.Controllers
{

    public abstract class CommandCrudControllerBase<TKey, TDtoModel, TCreateModel, TReadModel, TUpdateModel> : CommandControllerBase<TKey, TDtoModel, TCreateModel, TReadModel, TUpdateModel>
    {
        public  readonly IMapper _mapper;
        public CommandCrudControllerBase(IMediator mediator, IDistributedCache cache,
         IPrincipal httpContext, IAuthorizationService service, IMapper mapper) : base(mediator, cache, httpContext, service)
        {
            _mapper = mapper;
        }

        #region Search Properties

        public BasePagingFilter BuildPagingFilterFromDataTable([DataTablesRequest] DataTablesRequest requestModel)
        {
            var filter = _mapper.Map<BasePagingFilter>(requestModel);

            var QueryAndSearchFilter = MergeSearchAndDefaultFilter(filter.DynamicFilters, requestModel.Search?.Value);

            filter.DynamicFilters = QueryAndSearchFilter;

            filter.IncludeProperties = IncludePropertiesForListing;
            return filter;
        }

        public  async Task<ActionResult> ExecutePageFilter2ActionResult([DataTablesRequest] DataTablesRequest requestModel, BasePagingFilter filter)
        {
            var response = await PagedQuery(filter);

            return Json(new
            {
                draw = requestModel.Draw,
                recordsFiltered = response.Total,// response.PageCount,
                recordsTotal = response.Total,
                data = response.Data,
                length = requestModel.Length,
                pageLength = requestModel.Length,
                //start = 1
            });
        }

        #endregion

        public virtual async Task<ActionResult> Get([DataTablesRequest] DataTablesRequest requestModel)
        {
            try
            {
                var filter = BuildPagingFilterFromDataTable(requestModel);

                return await ExecutePageFilter2ActionResult(requestModel, filter);

            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                msg += "a";
                return Json(msg);
            }
        }


        [HttpPost]
        public async Task<ActionResult> searchByName(String term)
        {
            try
            {
                List<EntityFilter> DynamicFilters = BuildSearchFilter(term);
                var filter = new BasePagingFilter()
                {
                    Page= 1,
                    PageSize = 10,
                    DynamicFilters = DynamicFilters,
                };

                filter.IncludeProperties = IncludePropertiesForListing;

                var response = await PagedQuery(filter);

                return Json(response.Data.ToList());

            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                msg += "a";
                return Json(msg);
            }
           
        }
    }
}