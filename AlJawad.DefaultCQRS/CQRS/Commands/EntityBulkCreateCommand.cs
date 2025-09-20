using System.Collections.Generic;
using System.Security.Principal;


namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityBulkCreateCommand<TModel, TReadModel>
         : EntityBulkModelCommand<TModel, TReadModel>
    {
        public EntityBulkCreateCommand(IPrincipal principal, IEnumerable<TModel> model) : base(principal, model)
        {

        }
    }
}
