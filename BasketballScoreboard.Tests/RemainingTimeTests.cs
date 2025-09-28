using BasketballScoreboard.Components;
using Bunit;

namespace BasketballScoreboard.Tests;

public class RemainingTimeTests : TestContext
{
    [Fact]
    public void RemainingTimeIsNotSetInTheBeginningOfTheGame()
    {
        var component = RenderComponent<RemaningTime>();

        var timeParts = component.FindAll(".white-text").Select(tp => tp.TextContent.Trim());
        Assert.Contains(":", timeParts);
        Assert.Equal(2, timeParts.Count(tp => tp == "00"));
    }

    [Fact]
    public void OvertimeLengthIsConstant()
    {
        var component = RenderComponent<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());
        
        var timeParts = component.FindAll(".white-text").Select(tp => tp.TextContent.Trim());
        Assert.Contains(":", timeParts);
        Assert.Equal(1, timeParts.Count(tp => tp == "00"));
        Assert.Equal(1, timeParts.Count(tp => tp == "05"));
    }

    [Fact]
    public void RemainingTimeCanBeReset()
    {
        var component = RenderComponent<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());
        component.InvokeAsync(() => component.Instance.Reset());
        
        var timeParts = component.FindAll(".white-text").Select(tp => tp.TextContent.Trim());
        Assert.Contains(":", timeParts);
        Assert.Equal(2, timeParts.Count(tp => tp == "00"));
    }
}
