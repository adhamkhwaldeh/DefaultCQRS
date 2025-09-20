using AlJawad.DefaultCQRS.CQRS.Queries;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public class EntityMultiLocationsListQuery<TEntity,TResponseModel> : PrincipalQueryBase<TResponseModel>
    {

        public BaseQueryableFilter Filter { get; set; }

        public EntityMultiLocationsListQuery(IPrincipal principal)
            : base(principal)
        {
        }

        public EntityMultiLocationsListQuery(IPrincipal principal, BaseQueryableFilter filter)
            : base(principal)
        {
            this.Filter = filter;
        }

    }
}
