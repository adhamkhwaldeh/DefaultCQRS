using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityDisabledCommand<TKey, TReadModel>
       : PrincipalCommandBase<TReadModel>
    {
        public BaseIdentifierFilter<TKey> BaseFilter { get; }
        public IEnumerable<Claim> Claims { get; set; }

        public EntityDisabledCommand(IPrincipal principal, BaseIdentifierFilter<TKey> baseFilter) : base(principal)
        {
            BaseFilter = baseFilter;
            Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        }
    }
}
