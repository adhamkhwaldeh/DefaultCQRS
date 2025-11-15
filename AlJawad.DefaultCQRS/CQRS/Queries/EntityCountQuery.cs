using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public class EntityCountQuery<TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public BaseQueryableFilter Filter { get; set; }

        public EntityCountQuery(IPrincipal principal)
            : base(principal)
        {
        }

        public EntityCountQuery(IPrincipal principal, BaseQueryableFilter filter)
            : base(principal)
        {
            this.Filter = filter;
        }
    }
}
