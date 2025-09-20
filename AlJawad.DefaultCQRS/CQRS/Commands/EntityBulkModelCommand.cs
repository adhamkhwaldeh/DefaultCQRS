using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public abstract class EntityBulkModelCommand<TEntityModel, TReadModel> : PrincipalCommandBase<TReadModel>
    {
        public IEnumerable<TEntityModel> Model { get; set; }
        protected EntityBulkModelCommand(IPrincipal principal, IEnumerable<TEntityModel> model) : base(principal)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
   
    }
}
