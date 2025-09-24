using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class TeamScoreTests : TestContext
{
    [Fact]
    public void GameBeginsWithZeroPoints()
    {
        var component = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        Assert.Contains("00", component.Markup);
    }

    [Fact]
    public void ThereAreHomeAndAwayTeams()
    {
        var homeTeamComponent = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        var awayTeamComponent = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, false));

        Assert.Contains("HOME", homeTeamComponent.Markup);
        Assert.Contains("AWAY", awayTeamComponent.Markup);
    }

    [Fact]
    public void ScoreCanBeIncreased()
    {
        var component = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();

        Assert.Contains("01", component.Markup);
        Assert.Equal(1, component.Instance.Points);
    }

    [Fact]
    public void ScoreCanBeDecreased()
    {
        var component = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();

        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        Assert.Contains("01", component.Markup);
        Assert.Equal(1, component.Instance.Points);
    }

    [Fact]
    public void PointsCannotGoBelowZero()
    {
        var component = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }

    [Fact]
    public void ResettingReturnsToZeroPoints()
    {
        var component = RenderComponent<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();
        plusButton.MouseDown();

        component.InvokeAsync(() => component.Instance.Reset());

        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }
}
