using AlJawad.SqlDynamicLinker.DynamicFilter;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public class GetCountQuery<TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public BaseQueryableFilter Filter { get; set; }

        public GetCountQuery(IPrincipal principal)
            : base(principal)
        {
        }

        public GetCountQuery(IPrincipal principal, BaseQueryableFilter filter)
            : base(principal)
        {
            this.Filter = filter;
        }
    }
}
