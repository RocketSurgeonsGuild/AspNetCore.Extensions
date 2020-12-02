using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.AspNetCore.Contributions;
using Rocket.Surgery.AspNetCore.Conventions;
using Rocket.Surgery.AspNetCore.Filters;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.DependencyInjection;
using Rocket.Surgery.Conventions.Reflection;

[assembly: Convention(typeof(AspNetCoreConvention))]

namespace Rocket.Surgery.AspNetCore.Contributions
{
    /// <summary>
    /// Class MvcConvention.
    /// </summary>
    /// <seealso cref="IServiceConvention" />
    /// TODO Edit XML Comment Template for MvcConvention
    public class AspNetCoreConvention : IServiceConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="services">The services.</param>
        public void Register(IConventionContext context, IConfiguration configuration, IServiceCollection services)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var coreBuilder = services
                .AddMvcCore()
                .AddControllersAsServices()
                .AddApiExplorer();
            foreach (var item in context.AssemblyCandidateFinder.GetCandidateAssemblies("Rocket.Surgery.AspNetCore", "Microsoft.AspNetCore.Mvc"))
            {
                coreBuilder
                    .AddApplicationPart(item);
            }

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<NotFoundExceptionFilter>();
                options.Filters.Add<RequestExceptionFilter>();
            });
        }
    }
}
