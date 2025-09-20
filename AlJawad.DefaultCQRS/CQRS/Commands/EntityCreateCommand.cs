using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityCreateCommand<TModel, TReadModel>
         : EntityModelCommand<TModel, TReadModel>
    {
        public BaseFilter BaseFilter { get; }
        public IEnumerable<Claim> Claims { get; set; }
        public EntityCreateCommand(IPrincipal principal, BaseFilter baseFilter, TModel model) : base(principal, model)
        {
            BaseFilter = baseFilter;
            Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        }

    }
}
