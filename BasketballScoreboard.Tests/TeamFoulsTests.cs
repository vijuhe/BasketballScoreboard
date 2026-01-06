using Bunit;
using BasketballScoreboard.Components;
using AngleSharp.Dom;
using NUnit.Framework;

namespace BasketballScoreboard.Tests;

[TestFixture]
public class TeamFoulsTests : BunitContext
{
    [Test]
    public void ThereAreNoFoulsInTheBeginningOfTheGame()
    {
        using var component = Render<TeamFouls>();
        
        var foulDots = component.FindAll(".foul-dot");
        for (int i = 0; i < 5; i++)
        {
            AssertNoFoul(foulDots[i]);
        }
    }

    [Test]
    public void FoulsCanBeAdded()
    {
        using var component = Render<TeamFouls>();

        var foulDots = component.FindAll(".foul-dot");
        
        foulDots[4].MouseDown();
        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[3].MouseDown();
        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[2].MouseDown();
        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[1].MouseDown();
        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[0].MouseDown();
        foulDots = component.FindAll(".foul-dot");
        AssertFoul(foulDots[0]);
        AssertFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);
    }

    [Test]
    public void ManyFoulsCanBeAddedWithOneClick()
    {
        using var component = Render<TeamFouls>();

        var foulDots = component.FindAll(".foul-dot");
        foulDots[3].MouseDown();
        
        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);
    }

    [Test]
    public void FoulsCanBeManuallyResetByClickingTheTitle()
    {
        using var component = Render<TeamFouls>();
        var foulDots = component.FindAll(".foul-dot");
        foulDots[3].MouseDown();

        var title = component.Find(".title");
        title.MouseDown();

        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertNoFoul(foulDots[4]);
    }

    [Test]
    public void ResettingReturnsToZeroFouls()
    {
        using var component = Render<TeamFouls>();
        var foulDots = component.FindAll(".foul-dot");
        foulDots[1].MouseDown();

        component.InvokeAsync(() => component.Instance.Reset());

        foulDots = component.FindAll(".foul-dot");
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertNoFoul(foulDots[4]);
    }

    private static void AssertNoFoul(IElement element)
    {
        Assert.That(element.GetAttribute("style"), Does.Match("background-color: (grey|darkred);"));
    }

    private void AssertFoul(IElement element)
    {
        Assert.That(element.GetAttribute("style"), Does.Match("background-color: (white|red);"));
    }
}