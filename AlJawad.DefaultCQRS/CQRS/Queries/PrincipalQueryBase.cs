
using MediatR;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Queries
{
    public abstract class PrincipalQueryBase<TResponse> : IRequest<TResponse>
    {
        public IPrincipal Principal { get; set; }
        protected PrincipalQueryBase()
        {

        }

        protected PrincipalQueryBase(IPrincipal principal)
        {
            Principal = principal;
        }

    }
}
