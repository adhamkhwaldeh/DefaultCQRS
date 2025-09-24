using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.DefaultCQRS.Helper;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AlJawad.DefaultCQRS.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Http;
using AlJawad.DefaultCQRS.CQRS.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AlJawad.SqlDynamicLinker.Models;
using Swashbuckle.AspNetCore.Filters;
using AlJawad.SqlDynamicLinker.Swagger;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.Controllers
{

    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(ExceptionDto))]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class DefaultCommandControllerBase<TKey,TEntity,TDtoModel,TCreateModel, TReadModel, TUpdateModel> :
        CommandControllerBase<TKey, TDtoModel, TCreateModel, TReadModel, TUpdateModel>
    {
        public readonly IMapper Mapper;

        public DefaultCommandControllerBase(IMediator mediator,
            IDistributedCache appCache, IPrincipal httpContextAccessor,
            IAuthorizationService service, IMapper mapper)
            : base(mediator, appCache, httpContextAccessor, service)
        {
            Mapper = mapper;
        }
    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Details/{id}")]
        public virtual async Task<ActionResult<Response<TReadModel>>> GetAsync(TKey id, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.ViewDetailsAction)).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                var result = await Find(new BaseIdentifierFilter<TKey>()
                {
                    Id = id,
                    IncludeProperties = IncludePropertiesForDetails
                }, cancellationToken);
                return result.AsActionResult();
            }
            return new ChallengeResult();
        }

        [SwaggerRequestExample(typeof(EntityFilter), typeof(FilterExamples))]
        [SwaggerRequestExample(typeof(EntityMultilpleConditionsFilter), typeof(FilterCollectionExample))]
        [SwaggerRequestExample(typeof(EntityGeometryFilter), typeof(FilterGeometryExamples))]
        //TODO need to be checked
        //[SwaggerRequestExample(typeof(BaseQueryableFilter), typeof(BaseQueryableFilterExample))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("All")]
        public virtual async Task<ActionResult<ResponseArray<TReadModel>>> GetListAsync([FromQuery] BaseQueryableFilter baseFilter, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.ViewListAction)).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                baseFilter.DynamicFilters = MergeSearchAndDefaultFilter(baseFilter.DynamicFilters, baseFilter.Query);
                
                //if ((baseFilter.IncludeProperties != null) && (!baseFilter.IncludeProperties.Any()))
                if (baseFilter.IncludeProperties == null)
                {
                    baseFilter.IncludeProperties = IncludePropertiesForListing;
                }
                var result = await ListQuery(baseFilter, cancellationToken);
                return result.AsActionResult<TReadModel, TReadModel>();
            }
            return new ChallengeResult();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Page")]
        public virtual async Task<ActionResult<ResponseList<TReadModel>>> GetPagedListAsync([FromQuery] BasePagingFilter baseFilter, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.ViewListAction)).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                baseFilter.DynamicFilters = MergeSearchAndDefaultFilter(baseFilter.DynamicFilters, baseFilter.Query);
                //if ((baseFilter.IncludeProperties != null) && (!baseFilter.IncludeProperties.Any()))
                if (baseFilter.IncludeProperties == null)
                {
                    baseFilter.IncludeProperties = IncludePropertiesForListing;
                }
                var result = await PagedQuery(baseFilter, cancellationToken);
                return result.AsActionResult();
            }
            return new ChallengeResult();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Add")]
        public virtual async Task<ActionResult<TReadModel>> AddItemAsync(TCreateModel model, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.CreateAction, Mapper.Map<TEntity>(model))).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                var result = await Create(new BaseFilter()
                {
                    IncludeProperties = IncludePropertiesForManage
                }, model, cancellationToken);
                return result.GenerateResponse<TReadModel>();
            }
            return new ChallengeResult();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("Update/{id}")]
        public virtual async Task<ActionResult<TReadModel>> UpdateItemAsync(TKey id, TUpdateModel model, CancellationToken cancellationToken)
        {
            //var readModel = Mapper.Map<TReadModel>(model);
            //var entityModel = Mapper.Map<TEntity>(readModel);
            //var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.UpdateAction, entityModel, id)).ConfigureAwait(false);
            //if (auth.Succeeded)
            //{
                var result = await Update(new BaseIdentifierFilter<TKey>()
                {
                    Id = id,
                    IncludeProperties = IncludePropertiesForManage
                }, model, cancellationToken);

            return result.GenerateResponse<TReadModel>();
            //}
            //return new ChallengeResult();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("Delete/{id}")]
        public virtual async Task<ActionResult<TReadModel>> DeleteItemAsync(TKey id, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.DeleteAction,id)).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                var result = await Delete(new BaseIdentifierFilter<TKey>()
                {
                    Id = id,
                    IncludeProperties = IncludePropertiesForDetails
                }, cancellationToken);
                return result.GenerateResponse<TReadModel>();
            }
            return new ChallengeResult();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("Disable/{id}")]
        public virtual async Task<ActionResult<TReadModel>> DisableItemAsync(TKey id, CancellationToken cancellationToken)
        {
            var auth = await AuthorizationService.AuthorizeAsync(User, null, new BaseRequirement<TEntity,TKey>(BaseRequirementActions.DisableAction,id)).ConfigureAwait(false);
            if (auth.Succeeded)
            {
                var result = await Disable(new BaseIdentifierFilter<TKey>()
                {
                    Id = id,
                    IncludeProperties = IncludePropertiesForDetails
                }, cancellationToken);
                return result.GenerateResponse<TReadModel>();
            }
            return new ChallengeResult();
        }

    }
}
