using AlJawad.DefaultCQRS.CQRS.Permissions;
using AlJawad.DefaultCQRS.CQRS;
using AlJawad.DefaultCQRS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.ModelBinder;

namespace AlJawad.DefaultCQRS.Extensions
{
    public static class DefaultCQRSExtensions
    {
        public static void InitializeDefaultCQRS(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Add(new BaseQueryableFilterBinderProvider());
                options.ModelBinderProviders.Add(new BasePagingFilterBinderProvider());
            });

        }
    }
}
