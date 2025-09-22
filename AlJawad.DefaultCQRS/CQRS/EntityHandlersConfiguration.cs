using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.CQRS.Permissions;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using System;

namespace AlJawad.DefaultCQRS.CQRS
{

    public class EntityHandlersConfiguration<
    TEntityModel,
    TKeyModel,
    TCreateModel,
    TUpdateModel,
    TReadModel>
        where TEntityModel : class, IHaveIdentifier<TKeyModel>, new()
        where TReadModel : class
    {
        public Type? CreateCommandHandler { get; private set; }
        public Type? CreateCommandValidator { get; private set; }

        public Type? UpdateCommandHandler { get; private set; }
        public Type? UpdateCommandValidator { get; private set; }

        public Type? DeleteCommandHandler { get; private set; }

        public Type? AuthorizationHandler { get; private set; }

        public Type? IdentifierQueryHandler { get; private set; }
        public Type? ListQueryHandler { get; private set; }
        public Type? PagedQueryHandler { get; private set; }

        // --- Handlers with compile-time constraints ---

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithCreateHandler<THandler>()

            where  THandler : EntityCreateCommandHandler<IUnitOfWork, TEntityModel, TKeyModel, TCreateModel, TReadModel>
        {
            CreateCommandHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithUpdateHandler<THandler>()
            where THandler : EntityUpdateCommandHandler<IUnitOfWork, TEntityModel, TKeyModel, TUpdateModel, TReadModel>
        {
            UpdateCommandHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithDeleteHandler<THandler>()
            where THandler : EntityDeleteCommandHandler<IUnitOfWork, TEntityModel, TKeyModel,TReadModel>
        {
            DeleteCommandHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithAuthorizationHandler<THandler>()
            where THandler : AuthorizationHandler<BaseRequirement<TEntityModel, TKeyModel>>
        {
            AuthorizationHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithIdentifierQueryHandler<THandler>()
            where THandler : EntityIdentifierQueryHandler<IUnitOfWork,TEntityModel, TKeyModel, TReadModel>
        {
            IdentifierQueryHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithListQueryHandler<THandler>()
            where THandler : EntityListQueryHandler<IUnitOfWork, TEntityModel, TReadModel>
        {
            ListQueryHandler = typeof(THandler);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithPagedQueryHandler<THandler>()
            where THandler : EntityPagedQueryHandler<IUnitOfWork,TEntityModel, TReadModel>
        {
            PagedQueryHandler = typeof(THandler);
            return this;
        }

        // --- Validators (you can constrain them to FluentValidation AbstractValidator<T> if needed) ---

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithCreateValidator<TValidator>()
        {
            CreateCommandValidator = typeof(TValidator);
            return this;
        }

        public EntityHandlersConfiguration<TEntityModel, TKeyModel, TCreateModel, TUpdateModel, TReadModel>
            WithUpdateValidator<TValidator>()
        {
            UpdateCommandValidator = typeof(TValidator);
            return this;
        }
    }

}