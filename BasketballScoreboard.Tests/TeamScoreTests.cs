using Bunit;
using BasketballScoreboard.Components;
using NUnit.Framework;

namespace BasketballScoreboard.Tests;

[TestFixture]
public class TeamScoreTests : BunitContext
{
    [Test]
    public void GameBeginsWithZeroPoints()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        Assert.That(component.Markup, Does.Contain("00"));
    }

    [Test]
    public void ThereAreHomeAndAwayTeams()
    {
        using var homeTeamComponent = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        using var awayTeamComponent = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, false));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(homeTeamComponent.Markup, Does.Contain("HOME"));
            Assert.That(awayTeamComponent.Markup, Does.Contain("AWAY"));
        }
    }

    [Test]
    public void ScoreCanBeIncreased()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(component.Markup, Does.Contain("01"));
            Assert.That(component.Instance.Points, Is.EqualTo(1));
        }
    }

    [Test]
    public void ScoreCanBeDecreased()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();

        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(component.Markup, Does.Contain("01"));
            Assert.That(component.Instance.Points, Is.EqualTo(1));
        }
    }

    [Test]
    public void PointsCannotGoBelowZero()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));

        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(component.Markup, Does.Contain("00"));
            Assert.That(component.Instance.Points, Is.EqualTo(0));
        }
    }

    [Test]
    public void ResettingReturnsToZeroPoints()
    {
        using var component = Render<TeamScore>(parameters => parameters.Add(p => p.IsHomeTeam, true));
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();
        plusButton.MouseDown();

        component.InvokeAsync(() => component.Instance.Reset());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(component.Markup, Does.Contain("00"));
            Assert.That(component.Instance.Points, Is.EqualTo(0));
        }
    }
}
