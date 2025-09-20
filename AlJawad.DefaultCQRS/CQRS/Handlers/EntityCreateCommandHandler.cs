using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public class EntityCreateCommandHandler<TUnitOfWork, TEntity, TKey, TModel, TReadModel>
        : EntityDataContextHandlerBase<TUnitOfWork, TEntity, TKey, TReadModel, EntityCreateCommand<TModel, Response<TReadModel>>, Response<TReadModel>>
        where TUnitOfWork : IUnitOfWork
        where TEntity : class, IHaveIdentifier<TKey>, new()
        where TReadModel : class
    {
        private static short CodeType { get; set; }

        public EntityCreateCommandHandler(ILoggerFactory loggerFactory,
            TUnitOfWork dataContext,
            IMapper mapper,
            IHttpContextAccessor context,
            IDistributedCache cache,
            IServiceProvider provider)
            : base(loggerFactory, dataContext, mapper, context, cache, provider)
        {
        }

        protected override async Task<Response<TReadModel>> ProcessAsync(EntityCreateCommand<TModel, Response<TReadModel>> request, CancellationToken cancellationToken)
        {
            var EntityResponse = new Response<TReadModel>();
            try
            {
                var entity = Mapper.Map<TEntity>(request.Model);
                var dbSet = DataContext.Set<TEntity>();
                //TODO need to be reviewed
                //if (typeof(TModel).Implements<ICode>())
                //{
                //    CodeType = (short)request.Model.GetType().GetProperty("ConfigType")
                //           ?.GetValue(request.Model, null);
                //    if (CodeType != 0) entity = GenerateCode(entity, CodeType);
                //}
                await dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);

                await DataContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
                EntityResponse.StatusCode = StatusCodes.Status200OK;
                EntityResponse.ReturnStatus = true;
                EntityResponse.Data = await Read(entity.Id, request.BaseFilter.IncludeProperties, cancellationToken);
                DataContext.CommitTransaction();

                if (typeof(TEntity).Implements<ICache>())
                    Cache.Remove(typeof(TReadModel).Name);
            }
            catch (Exception ex)
            {

                EntityResponse.StatusCode = StatusCodes.Status422UnprocessableEntity;
                EntityResponse.ReturnStatus = false;
                EntityResponse.ReturnMessage.Add(string.Format("Unable to Insert Record {0}" + ex.Message, typeof(TEntity).Name));
                if (ex.InnerException != null)
                {
                    EntityResponse.ReturnMessage.Add(ex.InnerException.Message);
                }
                DataContext.RollbackTransaction();
                //throw new RectaDomainException(StatusCodes.Status422UnprocessableEntity, string.Format("Unable to Insert Record {0}" + ex.Message, typeof(TEntity).Name), ex);              

            }
            return EntityResponse;

        }
    }
}