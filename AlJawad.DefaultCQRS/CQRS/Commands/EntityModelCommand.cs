using System;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public abstract class EntityModelCommand<TEntityModel, TReadModel> : PrincipalCommandBase<TReadModel>
    {
        public TEntityModel Model { get; set; }
        public string Roles { get; set; }

        protected EntityModelCommand(IPrincipal principal, TEntityModel model) : base(principal)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;

        }
        protected EntityModelCommand(IPrincipal principal, TEntityModel model, string roles) : base(principal)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
            if (string.IsNullOrEmpty(roles)) throw new ArgumentNullException("Roles is empty.,");

        }
    

    }
}
