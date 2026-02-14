using FluentAssertions;
using MercuryBankApi.Generated;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for AR invoice operations against the Mercury sandbox.
/// </summary>
public class InvoiceTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public InvoiceTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetInvoicesAsync_ReturnsInvoicesList()
    {
        try
        {
            var invoices = await _sandbox.Client.GetInvoicesAsync();

            invoices.Should().NotBeNull();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            // Sandbox may not support AR invoice endpoints
        }
    }

    [SandboxFact]
    public async Task GetInvoiceAsync_ReturnsSingleInvoice()
    {
        IReadOnlyList<ApiV1ArInvoicesData> invoices;
        try
        {
            invoices = await _sandbox.Client.GetInvoicesAsync();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            return;
        }

        if (invoices.Count == 0) return;

        var invoice = await _sandbox.Client.GetInvoiceAsync(invoices[0].Id);

        invoice.Should().NotBeNull();
        invoice.Id.Should().Be(invoices[0].Id);
    }
}
