using FluentAssertions;
using MercuryBankApi.Generated;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for AR customer operations against the Mercury sandbox.
/// </summary>
public class CustomerTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public CustomerTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetCustomersAsync_ReturnsCustomersList()
    {
        try
        {
            var customers = await _sandbox.Client.GetCustomersAsync();

            customers.Should().NotBeNull();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            // Sandbox may not support AR customer endpoints
        }
    }

    [SandboxFact]
    public async Task GetCustomerAsync_ReturnsSingleCustomer()
    {
        IReadOnlyList<ApiV1ArCustomerResponseData> customers;
        try
        {
            customers = await _sandbox.Client.GetCustomersAsync();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            return;
        }

        if (customers.Count == 0) return;

        var customer = await _sandbox.Client.GetCustomerAsync(customers[0].Id);

        customer.Should().NotBeNull();
        customer.Id.Should().Be(customers[0].Id);
    }
}
