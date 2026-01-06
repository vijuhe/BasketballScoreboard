using Bunit;
using BasketballScoreboard.Components;
using AngleSharp.Dom;

namespace BasketballScoreboard.Tests;

public class PeriodsTests : BunitContext
{
    [Fact]
    public void GameBeginsWithFirstPeriod()
    {
        using var component = Render<Periods>();

        var periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[0]);
        for (int i = 1; i < 5; i++)
        {
            AssertPeriodNotOngoing(periodButtons[i]);
        }
    }

    [Fact]
    public void PeriodCanBeManuallyChanged()
    {
        using var component = Render<Periods>();

        var periodButtons = component.FindAll(".clickable");
        periodButtons[1].MouseDown();

        periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[1]);
        AssertPeriodNotOngoing(periodButtons[0]);
        AssertPeriodNotOngoing(periodButtons[2]);
        AssertPeriodNotOngoing(periodButtons[3]);
        AssertPeriodNotOngoing(periodButtons[4]);
    }

    [Fact]
    public void TryingToManuallyChangeToOngoingPeriodDoesNothing()
    {
        using var component = Render<Periods>();

        var periodButtons = component.FindAll(".clickable");
        periodButtons[0].MouseDown();

        periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[0]);
        for (int i = 1; i < 5; i++)
        {
            AssertPeriodNotOngoing(periodButtons[i]);
        }
    }

    [Fact]
    public void PeriodCanBeAutomaticallyChanged()
    {
        using var component = Render<Periods>();

        component.InvokeAsync(() => component.Instance.Next());
        component.InvokeAsync(() => component.Instance.Next());
    
        var periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[2]);
        AssertPeriodNotOngoing(periodButtons[0]);
        AssertPeriodNotOngoing(periodButtons[1]);
        AssertPeriodNotOngoing(periodButtons[3]);
        AssertPeriodNotOngoing(periodButtons[4]);
    }

    [Fact]
    public void FourthPeriodAndOvertimeAreLastPeriods()
    {
        using var component = Render<Periods>();

        Assert.False(component.Instance.IsLastPeriod);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.False(component.Instance.IsLastPeriod);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.False(component.Instance.IsLastPeriod);
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.True(component.Instance.IsLastPeriod);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.True(component.Instance.IsLastPeriod);
    }

    [Fact]
    public void ThereIsNoNextPeriodAfterOvertime()
    {
        using var component = Render<Periods>();

        for(int i = 0; i < 9; i++)
        {
            component.InvokeAsync(() => component.Instance.Next());
        }

        var periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[4]);
        AssertPeriodNotOngoing(periodButtons[0]);
        AssertPeriodNotOngoing(periodButtons[1]);
        AssertPeriodNotOngoing(periodButtons[2]);
        AssertPeriodNotOngoing(periodButtons[3]);
    }

    [Fact]
    public void ResettingReturnsToFirstPeriod()
    {
        using var component = Render<Periods>();
        component.InvokeAsync(() => component.Instance.Next());
        component.InvokeAsync(() => component.Instance.Next());

        component.InvokeAsync(() => component.Instance.Reset());

        var periodButtons = component.FindAll(".clickable");
        AssertPeriodOngoing(periodButtons[0]);
        for (int i = 1; i < 5; i++)
        {
            AssertPeriodNotOngoing(periodButtons[i]);
        }
    }

    private static void AssertPeriodNotOngoing(IElement element)
    {
        Assert.DoesNotContain("background-color: red;", element.GetAttribute("style"));
    }

    private static void AssertPeriodOngoing(IElement element)
    {
        Assert.Contains("background-color: red;", element.GetAttribute("style"));
    }
}