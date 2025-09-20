using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.UnitOfWork;

namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public abstract class DataContextHandlerBase<TUnitOfWork, TRequest, TResponse>
       : RequestHandlerBase<TRequest, TResponse>
       where TUnitOfWork : IUnitOfWork
       where TRequest : IRequest<TResponse>
    {
        protected DataContextHandlerBase(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory)
        {
            Assert.NotNull(loggerFactory, nameof(loggerFactory));
            Assert.NotNull(mapper, nameof(mapper));
            DataContext = dataContext;
            Mapper = mapper;
        }
        protected DataContextHandlerBase(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper, IHttpContextAccessor context)
            : base(loggerFactory)
        {
            Assert.NotNull(loggerFactory, nameof(loggerFactory));
            Assert.NotNull(mapper, nameof(mapper));
            Assert.NotNull(context, nameof(context));
            DataContext = dataContext;
            Mapper = mapper;
            Context = context;
        }
        protected IHttpContextAccessor Context { get; set; }
        protected TUnitOfWork DataContext { get; }

        protected IMapper Mapper { get; }
    }
}
