using System.Net.Http.Headers;
using MercuryBankApi.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MercuryBankApi;

/// <summary>
/// Extension methods for registering Mercury Bank API services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Mercury Bank API client to the service collection.
    /// Binds configuration from the "Mercury" section by default.
    /// </summary>
    public static IServiceCollection AddMercuryBankApi(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = MercuryBankOptions.SectionName)
    {
        services.Configure<MercuryBankOptions>(configuration.GetSection(sectionName));

        services.AddHttpClient<IMercuryApiClient, MercuryApiClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<MercuryBankOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            if (!string.IsNullOrEmpty(options.ApiToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.ApiToken);
            }
        });

        services.AddTransient<IMercuryBankClient, MercuryBankClient>();

        return services;
    }

    /// <summary>
    /// Adds the Mercury Bank API client with inline options configuration.
    /// </summary>
    public static IServiceCollection AddMercuryBankApi(
        this IServiceCollection services,
        Action<MercuryBankOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<IMercuryApiClient, MercuryApiClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<MercuryBankOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            if (!string.IsNullOrEmpty(options.ApiToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.ApiToken);
            }
        });

        services.AddTransient<IMercuryBankClient, MercuryBankClient>();

        return services;
    }
}
