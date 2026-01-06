using Bunit;
using TimeoutComponent = BasketballScoreboard.Components.Timeout;

namespace BasketballScoreboard.Tests;

public class TimeoutTests : BunitContext
{
    [Fact]
    public void TimeoutIsNotOngoingInTheBeginningOfTheGame()
    {
        var component = Render<TimeoutComponent>();

        Assert.Contains("TIME OUT", component.Markup);
    }

    [Fact]
    public void TimeoutCanBeStarted()
    {
        var component = Render<TimeoutComponent>();

        var textElement = component.Find(".text");
        textElement.MouseDown();

        Assert.DoesNotContain("TIME OUT", component.Markup);
        Assert.Contains("01:00", component.Markup);
    }

    [Fact]
    public void TimeoutCanBeCancelled()
    {
        var component = Render<TimeoutComponent>();
        var textElement = component.Find(".text");
        textElement.MouseDown();
        
        textElement.MouseDown();

        Assert.Contains("TIME OUT", component.Markup);
    }
}