using MediatR;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public class EntityIdentifierQuery<TKey, TReadModel> : PrincipalQueryBase<TReadModel>
    {
        public BaseIdentifierFilter<TKey> Filter { get; }
        //public string IncludeProperties { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        //public string ConstGetAll => @"EmployeeWorkHistories,EmployeeWorkHistories.Department,EmployeeWorkHistories.Designation,EmployeeWorkHistories.Department,User,User.UserRoles,EmployeeStatus,User.UserAccessRigths";

        //public long EmployeeId { get; set; }

        //public long ClientId { get; set; }

        public EntityIdentifierQuery() : base()
        {

        }

        public EntityIdentifierQuery(IPrincipal principal, BaseIdentifierFilter<TKey> filter)
        : base(principal)
        {
            Filter = filter;
            Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
            //TODO: need to be handled
            //if (Claims.Any())
            //{
            //    EmployeeId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.EmployeeId).FirstOrDefault().Value);
            //    ClientId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.ClientId).FirstOrDefault().Value);
            //}
        }
        
        //public EntityIdentifierQuery(IPrincipal principal, TKey id, string includeProperties)
        //   : base(principal)
        //{
        //    Id = id;
        //    IncludeProperties = includeProperties;
        //    Claims = ((ClaimsIdentity)Principal.Identity)?.Claims.AsEnumerable();
        //    //TODO: need to be handled
        //    //if (Claims.Any())
        //    //{
        //    //    EmployeeId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.EmployeeId).FirstOrDefault().Value);
        //    //    ClientId = Convert.ToInt64(Claims.Where(x => x.Type == TokenConstants.Claims.ClientId).FirstOrDefault().Value);
        //    //}
        //}
     }
}