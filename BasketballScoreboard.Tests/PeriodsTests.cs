using Bunit;
using BasketballScoreboard.Components;
using AngleSharp.Dom;
using NUnit.Framework;

namespace BasketballScoreboard.Tests;

[TestFixture]
public class PeriodsTests : BunitContext
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
    public void FourthPeriodAndOvertimeAreLastPeriods()
    {
        using var component = Render<Periods>();

        Assert.That(component.Instance.IsLastPeriod, Is.False);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.That(component.Instance.IsLastPeriod, Is.False);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.That(component.Instance.IsLastPeriod, Is.False);
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.That(component.Instance.IsLastPeriod, Is.True);
        component.InvokeAsync(() => component.Instance.Next());
        Assert.That(component.Instance.IsLastPeriod, Is.True);
    }

    [Test]
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

    [Test]
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
        Assert.That(element.GetAttribute("style"), Does.Not.Contain("background-color: red;"));
    }

    private static void AssertPeriodOngoing(IElement element)
    {
        Assert.That(element.GetAttribute("style"), Does.Contain("background-color: red;"));
    }
}