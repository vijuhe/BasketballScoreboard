using Bunit;
using NUnit.Framework;
using TimeoutComponent = BasketballScoreboard.Components.Timeout;

namespace BasketballScoreboard.Tests;

[TestFixture]
public class TimeoutTests : BunitContext
{
    [Test]
    public void TimeoutIsNotOngoingInTheBeginningOfTheGame()
    {
        var component = Render<TimeoutComponent>();

        Assert.That(component.Markup, Does.Contain("TIME OUT"));
    }

    [Test]
    public void TimeoutCanBeStarted()
    {
        var component = Render<TimeoutComponent>();

        var textElement = component.Find(".text");
        textElement.MouseDown();

        Assert.That(component.Markup, Does.Not.Contain("TIME OUT"));
        Assert.That(component.Markup, Does.Contain("01:00"));
    }

    [Test]
    public void TimeoutCanBeCancelled()
    {
        var component = Render<TimeoutComponent>();
        var textElement = component.Find(".text");
        textElement.MouseDown();
        
        textElement.MouseDown();

        Assert.That(component.Markup, Does.Contain("TIME OUT"));
    }
}