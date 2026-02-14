using FluentAssertions;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for event operations against the Mercury sandbox.
/// </summary>
public class EventTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public EventTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetEventsAsync_ReturnsEventsList()
    {
        var events = await _sandbox.Client.GetEventsAsync();

        events.Should().NotBeNull();
    }

    [SandboxFact]
    public async Task GetEventAsync_ReturnsSingleEvent()
    {
        var events = await _sandbox.Client.GetEventsAsync();
        if (events.Count == 0) return;

        var ev = await _sandbox.Client.GetEventAsync(events[0].Id);

        ev.Should().NotBeNull();
        ev.Id.Should().Be(events[0].Id);
    }
}
