using AlJawad.DefaultCQRS.CQRS;
using AlJawad.DefaultCQRS.CQRS.Behaviors;
using AlJawad.DefaultCQRS.CQRS.Commands;
using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.CQRS.Permissions;
using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AlJawad.DefaultCQRS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityDynamicConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel, TmpAuthorizationHandler>(this IServiceCollection services, IConfiguration Configuration, Action<EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>> options = null)
             where TEntityModel : class, IHaveIdentifier<TKeyModel>, new()
             where TReadModel : class
             where TmpAuthorizationHandler : AuthorizationHandler<BaseRequirement<TEntityModel, TKeyModel>>
        {

            var handlersConfiguration = new EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>();
            options?.Invoke(handlersConfiguration);

            #region DI Scope and Transient

            var createCommandHandler = handlersConfiguration.CreateCommandHandler ?? typeof(EntityCreateCommandHandler<IUnitOfWork, TEntityModel, TKeyModel, TCreateModel, TReadModel>);
            services.AddTransient(typeof(IRequestHandler<EntityCreateCommand<TCreateModel, Response<TReadModel>>, Response<TReadModel>>), createCommandHandler);

            var createCommandValidator = handlersConfiguration.CreateCommandValidator ?? typeof(ValidateEntityModelCommandBehavior<TCreateModel,TReadModel>);
            services.AddTransient(typeof(IPipelineBehavior<EntityCreateCommand<TCreateModel, Response<TReadModel>>, Response<TReadModel>>), createCommandValidator);

            var updateCommandHandler = handlersConfiguration.UpdateCommandHandler ?? typeof(EntityUpdateCommandHandler<IUnitOfWork, TEntityModel, TKeyModel, TUpdateModel, TReadModel>);
            services.AddTransient(typeof(IRequestHandler<EntityUpdateCommand<TKeyModel, TUpdateModel, Response<TReadModel>>, Response<TReadModel>>), updateCommandHandler);

            var updateCommandValidator = handlersConfiguration.UpdateCommandValidator ?? typeof(ValidateEntityModelCommandBehavior<TUpdateModel, TReadModel>);
            services.AddTransient(typeof(IPipelineBehavior<EntityUpdateCommand<TKeyModel, TUpdateModel, Response<TReadModel>>, Response<TReadModel>>), updateCommandValidator);

            var deleteCommandHandler = handlersConfiguration.DeleteCommandHandler ?? typeof(EntityDeleteCommandHandler<IUnitOfWork, TEntityModel, TKeyModel, TReadModel>);
            services.AddTransient(typeof(IRequestHandler<EntityDeleteCommand<TKeyModel, Response<TReadModel>>, Response<TReadModel>>), deleteCommandHandler);

            var authorizationHandler = handlersConfiguration.AuthorizationHandler ?? typeof(TmpAuthorizationHandler);
            services.AddTransient(typeof(IAuthorizationHandler), authorizationHandler);

            var identifierQueryHandler = handlersConfiguration.IdentifierQueryHandler ?? typeof(EntityIdentifierQueryHandler<IUnitOfWork, TEntityModel, TKeyModel, TReadModel>);
            services.AddScoped(typeof(IRequestHandler<EntityIdentifierQuery<TKeyModel, Response<TReadModel>>, Response<TReadModel>>), identifierQueryHandler);

            var listQueryHandler = handlersConfiguration.ListQueryHandler ?? typeof(EntityListQueryHandler<IUnitOfWork, TEntityModel, TReadModel>);
            services.AddScoped(typeof(IRequestHandler<EntityListQuery<ResponseArray<TReadModel>>, ResponseArray<TReadModel>>), listQueryHandler);

            var pagedQueryHandler = handlersConfiguration.PagedQueryHandler ?? typeof(EntityPagedQueryHandler<IUnitOfWork, TEntityModel, TReadModel>);
            services.AddScoped(typeof(IRequestHandler<EntityPagedQuery<ResponseList<TReadModel>>, ResponseList<TReadModel>>), pagedQueryHandler);
            #endregion
        }
    }
}
