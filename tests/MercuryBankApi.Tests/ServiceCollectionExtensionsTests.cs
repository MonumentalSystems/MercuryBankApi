using FluentAssertions;
using MercuryBankApi.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MercuryBankApi.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMercuryBankApi_WithConfiguration_RegistersServices()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Mercury:ApiToken"] = "test-token",
                ["Mercury:BaseUrl"] = "https://test.mercury.com/api/v1"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddMercuryBankApi(config);
        var provider = services.BuildServiceProvider();

        provider.GetService<IMercuryBankClient>().Should().NotBeNull();
        provider.GetService<IMercuryApiClient>().Should().NotBeNull();

        var options = provider.GetRequiredService<IOptions<MercuryBankOptions>>().Value;
        options.ApiToken.Should().Be("test-token");
        options.BaseUrl.Should().Be("https://test.mercury.com/api/v1");
    }

    [Fact]
    public void AddMercuryBankApi_WithAction_RegistersServices()
    {
        var services = new ServiceCollection();
        services.AddMercuryBankApi(opts =>
        {
            opts.ApiToken = "inline-token";
            opts.BaseUrl = "https://inline.test.com";
        });
        var provider = services.BuildServiceProvider();

        provider.GetService<IMercuryBankClient>().Should().NotBeNull();

        var options = provider.GetRequiredService<IOptions<MercuryBankOptions>>().Value;
        options.ApiToken.Should().Be("inline-token");
    }

    [Fact]
    public void MercuryBankOptions_DefaultValues()
    {
        var options = new MercuryBankOptions();

        options.BaseUrl.Should().Be("https://api.mercury.com/api/v1");
        options.ApiToken.Should().BeEmpty();
        MercuryBankOptions.SectionName.Should().Be("Mercury");
    }
}
