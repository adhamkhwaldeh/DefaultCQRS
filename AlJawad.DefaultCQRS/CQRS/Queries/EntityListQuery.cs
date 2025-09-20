using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Queries

{
    public class EntityListQuery<TReadModel> : PrincipalQueryBase<TReadModel>{

        public BaseQueryableFilter Filter { get; set; }

        public EntityListQuery(IPrincipal principal)
            : base(principal)
        {
        }

        public EntityListQuery(IPrincipal principal, BaseQueryableFilter filter)
            : base(principal)
        {
            this.Filter = filter;
        }

    }
}
