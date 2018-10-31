using System;
using Edison.ChatService.Repositories;
using Edison.Common;
using Edison.Common.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSingletonCosmosDBRepository<T>(this IServiceCollection services, string name) where T : class, IEntityChatDAO
        {
            services.AddSingleton<ICosmosDBRepositoryChat<T>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CosmosDBOptions>>().Value;
                var logger = sp.GetRequiredService<ILogger<CosmosDBRepositoryChat<T>>>();
                return new CosmosDBRepositoryChat<T>(options.Endpoint, options.AuthKey, options.Database, name, logger);
            });
            return services;
        }
    }
}
