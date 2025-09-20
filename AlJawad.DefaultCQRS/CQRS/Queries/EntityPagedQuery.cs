using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using AlJawad.SqlDynamicLinker.DynamicFilter;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public class EntityPagedQuery<TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public BasePagingFilter Filter { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public EntityPagedQuery(IPrincipal principal, BasePagingFilter filter)
            : base(principal)
        {
            Filter = filter;
            //Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
            
            
            //TODO: need to be handled
            //if (Claims.Any())
            //{
            //    EmployeeId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.EmployeeId).FirstOrDefault().Value);
            //    ClientId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.ClientId).FirstOrDefault().Value);
            //}
        }

        // public EntityQuery Query { get; }
        // public string IncludeProperties { get; set; }

        //public EntityPagedQuery(IPrincipal principal, EntityQuery query, string includeProperties)
        //  : base(principal)
        //{
        //    Query = query;
        //    IncludeProperties = includeProperties;
        //    Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        //    //TODO: need to be handled
        //    //if (Claims.Any())
        //    //{
        //    //    EmployeeId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.EmployeeId).FirstOrDefault().Value);
        //    //    ClientId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.ClientId).FirstOrDefault().Value);
        //    //}
        //}
  
        //public long EmployeeId { get; set; }
        //public long ClientId { get; set; }
        //public string ConstGetAll => @"EmployeeWorkHistories,EmployeeWorkHistories.Department,EmployeeWorkHistories.Designation,EmployeeWorkHistories.Department,User,User.UserRoles,EmployeeStatus,User.UserAccessRigths";
    }
}
