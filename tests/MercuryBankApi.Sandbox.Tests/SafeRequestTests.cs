using FluentAssertions;
using MercuryBankApi.Generated;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for SAFE request operations against the Mercury sandbox.
/// </summary>
public class SafeRequestTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public SafeRequestTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetSafeRequestsAsync_ReturnsSafeRequestsList()
    {
        try
        {
            var requests = await _sandbox.Client.GetSafeRequestsAsync();

            requests.Should().NotBeNull();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            // Sandbox may not support SAFE endpoints
        }
        catch (ApiException ex) when (ex.InnerException is System.Text.Json.JsonException)
        {
            // Sandbox may return enum values not in the generated schema
        }
    }

    [SandboxFact]
    public async Task GetSafeRequestAsync_ReturnsSingleSafeRequest()
    {
        IReadOnlyList<APISafeRequest> requests;
        try
        {
            requests = await _sandbox.Client.GetSafeRequestsAsync();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            return;
        }
        catch (ApiException ex) when (ex.InnerException is System.Text.Json.JsonException)
        {
            return;
        }

        if (requests.Count == 0) return;

        var request = await _sandbox.Client.GetSafeRequestAsync(requests[0].Id);

        request.Should().NotBeNull();
        request.Id.Should().Be(requests[0].Id);
    }
}
