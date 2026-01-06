using BasketballScoreboard.Components;
using Bunit;

namespace BasketballScoreboard.Tests;

public class RemainingTimeTests : BunitContext
{
    [Fact]
    public void RemainingTimeIsNotSetInTheBeginningOfTheGame()
    {
        using var component = Render<RemaningTime>();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void OvertimeLengthIsConstant()
    {
        using var component = Render<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());

        IEnumerable<string> timeParts = GetTimeParts(component);
        Assert.Contains("05", timeParts);
        Assert.Contains(":", timeParts);
        Assert.Contains("00", timeParts);
    }

    [Fact]
    public void RemainingTimeCanBeReset()
    {
        using var component = Render<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());
        component.InvokeAsync(() => component.Instance.Reset());

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void RemainingTimeCannotBeSetBelowZero()
    {
        using var component = Render<RemaningTime>();

        var minusButtons = component.FindAll(".adjustment:contains('-')");
        minusButtons[0].Click();
        minusButtons[1].Click();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Fact]
    public void RemainingTimeCanBeChanged()
    {
        using var component = Render<RemaningTime>();

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
