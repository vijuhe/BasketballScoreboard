using BasketballScoreboard.Components;
using Bunit;
using NUnit.Framework;

namespace BasketballScoreboard.Tests;

[TestFixture]
public class RemainingTimeTests : BunitContext
{
    [Test]
    public void RemainingTimeIsNotSetInTheBeginningOfTheGame()
    {
        using var component = Render<RemaningTime>();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Test]
    public void OvertimeLengthIsConstant()
    {
        using var component = Render<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());

        IEnumerable<string> timeParts = GetTimeParts(component);
        Assert.That(timeParts, Does.Contain("05"));
        Assert.That(timeParts, Does.Contain(":"));
        Assert.That(timeParts, Does.Contain("00"));
    }

    [Test]
    public void RemainingTimeCanBeReset()
    {
        using var component = Render<RemaningTime>();
        
        component.InvokeAsync(() => component.Instance.Overtime());
        component.InvokeAsync(() => component.Instance.Reset());

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Test]
    public void RemainingTimeCannotBeSetBelowZero()
    {
        using var component = Render<RemaningTime>();

        var minusButtons = component.FindAll(".adjustment:contains('-')");
        minusButtons[0].Click();
        minusButtons[1].Click();

        IEnumerable<string> timeParts = GetTimeParts(component);
        AssertNoRemainingTime(timeParts);
    }

    [Test]
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
        Assert.That(timeParts, Does.Contain("02"));
        Assert.That(timeParts, Does.Contain(":"));
        Assert.That(timeParts, Does.Contain("01"));
    }

    private static void AssertNoRemainingTime(IEnumerable<string> timeParts)
    {
        Assert.That(timeParts, Does.Contain(":"));
        Assert.That(timeParts.Where(tp => tp == "00"), Has.Exactly(2).Items);
    }

    private static IEnumerable<string> GetTimeParts(IRenderedComponent<RemaningTime> component)
    {
        return component.FindAll(".white-text").Select(tp => tp.TextContent.Trim());
    }
}
