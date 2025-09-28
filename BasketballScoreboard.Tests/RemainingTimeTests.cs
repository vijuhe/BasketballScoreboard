using BasketballScoreboard.Components;
using Bunit;

namespace BasketballScoreboard.Tests;

public class RemainingTimeTests : TestContext
{
    [Fact]
    public void RemainingTimeIsNotSetInTheBeginningOfTheGame()
    {
        var component = RenderComponent<RemaningTime>();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void OvertimeLengthIsConstant()
    {
        var component = RenderComponent<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());

        IEnumerable<string> timeParts = GetTimeParts(component);
        Assert.Contains("05", timeParts);
        Assert.Contains(":", timeParts);
        Assert.Contains("00", timeParts);
    }

    [Fact]
    public void RemainingTimeCanBeReset()
    {
        var component = RenderComponent<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());
        component.InvokeAsync(() => component.Instance.Reset());

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void RemainingTimeCannotBeSetBelowZero()
    {
        var component = RenderComponent<RemaningTime>();

        var minusButtons = component.FindAll(".adjustment:contains('-')");
        minusButtons[0].Click();
        minusButtons[1].Click();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void RemainingTimeCanBeChanged()
    {
        var component = RenderComponent<RemaningTime>();

        var minusButtons = component.FindAll(".adjustment:contains('-')");
        var plusButtons = component.FindAll(".adjustment:contains('+')");
        plusButtons[0].Click();
        plusButtons[1].Click();
        plusButtons[0].Click();
        plusButtons[1].Click();
        plusButtons[0].Click();
        minusButtons[0].Click();
        minusButtons[1].Click();

        IEnumerable<string> timeParts = GetTimeParts(component);
        Assert.Contains("02", timeParts);
        Assert.Contains(":", timeParts);
        Assert.Contains("01", timeParts);
    }

    private static void AssertNoRemainingTime(IEnumerable<string> timeParts)
    {
        Assert.Contains(":", timeParts);
        Assert.Equal(2, timeParts.Count(tp => tp == "00"));
    }

    private static IEnumerable<string> GetTimeParts(IRenderedComponent<RemaningTime> component)
    {
        return component.FindAll(".white-text").Select(tp => tp.TextContent.Trim());
    }
}
