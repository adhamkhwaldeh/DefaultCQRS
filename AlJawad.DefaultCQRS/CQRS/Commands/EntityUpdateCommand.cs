using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityUpdateCommand<TKey, TModel, TReadModel>
       : EntityModelCommand<TModel, TReadModel>
    {
        public BaseIdentifierFilter<TKey>  BaseFilter { get; }
        public IEnumerable<Claim> Claims { get; set; }

        public EntityUpdateCommand(IPrincipal principal, BaseIdentifierFilter<TKey> baseFilter, TModel model) : base(principal, model)
        {
            BaseFilter = baseFilter;
            Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        }

      
    }
}
