using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class TeamScoreTests : BunitContext
{
    [Fact]
    public void GameBeginsWithZeroPoints()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        Assert.Contains("00", component.Markup);
    }

    [Fact]
    public void ThereAreHomeAndAwayTeams()
    {
        using var homeTeamComponent = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        using var awayTeamComponent = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, false));

        Assert.Contains("HOME", homeTeamComponent.Markup);
        Assert.Contains("AWAY", awayTeamComponent.Markup);
    }

    [Fact]
    public void ScoreCanBeIncreased()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();

        Assert.Contains("01", component.Markup);
        Assert.Equal(1, component.Instance.Points);
    }

    [Fact]
    public void ScoreCanBeDecreased()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

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
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }

    [Fact]
    public void ResettingReturnsToZeroPoints()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();
        plusButton.MouseDown();

        component.InvokeAsync(() => component.Instance.Reset());

        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }
}
