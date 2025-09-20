using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.CQRS.Commands;
using MassTransit;

namespace AlJawad.DefaultCQRS.CQRS
{
    public static class GeneralHandlerInitializer
    {
        public static IServiceCollection AddGeneralHandlers(this IServiceCollection services)
        {

            var assembly = typeof(GeneralHandlerInitializer).Assembly;
            services.AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
  
}