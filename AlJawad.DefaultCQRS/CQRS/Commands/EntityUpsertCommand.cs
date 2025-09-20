using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityUpsertCommand<TKey, TModel, TReadModel>
        : EntityModelCommand<TModel, TReadModel>
    {
        public EntityUpsertCommand(IPrincipal principal, TKey id, TModel model) : base(principal, model)
        {
            Id = id;
        }
        public TKey Id { get; }
    }
}