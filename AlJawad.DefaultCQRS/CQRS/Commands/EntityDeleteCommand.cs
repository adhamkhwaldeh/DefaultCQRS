using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public class EntityDeleteCommand<TKey, TReadModel>
       : PrincipalCommandBase<TReadModel>
    {

        public BaseIdentifierFilter<TKey> BaseFilter { get; }
        public IEnumerable<Claim> Claims { get; set; }

        public EntityDeleteCommand(IPrincipal principal, BaseIdentifierFilter<TKey> baseFilter) : base(principal)
        {
            BaseFilter = baseFilter;
            Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        }

        //public EntityDeleteCommand(IPrincipal principal, BaseIdentifierFilter<TKey> BaseFilter ) : base(principal, BaseFilter)
        //{

        //}
       

    }
}
