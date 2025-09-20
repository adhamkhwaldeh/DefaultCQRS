using MediatR;
using System.Security.Principal;

namespace AlJawad.DefaultCQRS.CQRS.Commands
{
    public abstract class PrincipalCommandBase<TResponse> : IRequest<TResponse>
    {
        protected PrincipalCommandBase(IPrincipal principal)
        {
            Principal = principal;
        }
        public IPrincipal Principal { get; }
    }
}
