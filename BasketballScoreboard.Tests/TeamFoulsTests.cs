using Bunit;
using BasketballScoreboard.Components;
using AngleSharp.Dom;

namespace BasketballScoreboard.Tests;

public class TeamFoulsTests : TestContext
{
    [Fact]
    public void ThereAreNoFoulsInTheBeginningOfTheGame()
    {
        var component = RenderComponent<TeamFouls>();
        
        var foulDots = component.FindAll(".foul-dot");
        for (int i = 0; i < 5; i++)
        {
            AssertNoFoul(foulDots[i]);
        }
    }

    [Fact]
    public void FoulsCanBeAdded()
    {
        var component = RenderComponent<TeamFouls>();

        var foulDots = component.FindAll(".foul-dot");
        
        foulDots[4].MouseDown();
        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[3].MouseDown();
        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[2].MouseDown();
        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[1].MouseDown();
        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);

        foulDots[0].MouseDown();
        foulDots.Refresh();
        AssertFoul(foulDots[0]);
        AssertFoul(foulDots[1]);
        AssertFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);
    }

    [Fact]
    public void ManyFoulsCanBeAddedWithOneClick()
    {
        var component = RenderComponent<TeamFouls>();

        var foulDots = component.FindAll(".foul-dot");
        foulDots[3].MouseDown();
        
        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertFoul(foulDots[3]);
        AssertFoul(foulDots[4]);
    }

    [Fact]
    public void FoulsCanBeManuallyResetByClickingTheTitle()
    {
        var component = RenderComponent<TeamFouls>();
        var foulDots = component.FindAll(".foul-dot");
        foulDots[3].MouseDown();

        var title = component.Find(".title");
        title.MouseDown();

        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertNoFoul(foulDots[4]);
    }

    [Fact]
    public void ResettingReturnsToZeroFouls()
    {
        var component = RenderComponent<TeamFouls>();
        var foulDots = component.FindAll(".foul-dot");
        foulDots[1].MouseDown();

        component.InvokeAsync(() => component.Instance.Reset());

        foulDots.Refresh();
        AssertNoFoul(foulDots[0]);
        AssertNoFoul(foulDots[1]);
        AssertNoFoul(foulDots[2]);
        AssertNoFoul(foulDots[3]);
        AssertNoFoul(foulDots[4]);
    }

    private static void AssertNoFoul(IElement element)
    {
        Assert.Matches("background-color: (grey|darkred);", element.GetAttribute("style"));
    }

    private void AssertFoul(IElement element)
    {
        Assert.Matches("background-color: (white|red);", element.GetAttribute("style"));
    }
}