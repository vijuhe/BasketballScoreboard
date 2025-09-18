using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class TeamScoreTests : TestContext
{
    [Fact]
    public void TeamScore_InitialState_ShouldHaveZeroPoints()
    {
        // Arrange & Act
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, true));

        // Assert
        Assert.Contains("HOME", component.Markup);
        Assert.Contains("00", component.Markup);
    }

    [Fact]
    public void TeamScore_AwayTeam_ShouldDisplayAwayTitle()
    {
        // Arrange & Act
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, false));

        // Assert
        Assert.Contains("AWAY", component.Markup);
        Assert.Contains("text-align: right;", component.Markup);
    }

    [Fact]
    public void TeamScore_ClickPlusButton_ShouldIncreaseScore()
    {
        // Arrange
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, true));

        // Act
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();

        // Assert
        Assert.Contains("01", component.Markup);
        Assert.Equal(1, component.Instance.Points);
    }

    [Fact]
    public void TeamScore_ClickMinusButton_ShouldDecreaseScore()
    {
        // Arrange
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, true));
        
        // First increase score to 2
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();

        // Act
        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        // Assert
        Assert.Contains("01", component.Markup);
        Assert.Equal(1, component.Instance.Points);
    }

    [Fact]
    public void TeamScore_ClickMinusButtonAtZero_ShouldRemainAtZero()
    {
        // Arrange
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, true));

        // Act
        var minusButton = component.Find(".adjustment:contains('-')");
        minusButton.MouseDown();

        // Assert
        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }

    [Fact]
    public void TeamScore_Reset_ShouldReturnToZero()
    {
        // Arrange
        var component = RenderComponent<TeamScore>(parameters => parameters
            .Add(p => p.IsHomeTeam, true));
        
        // Increase score first
        var plusButton = component.Find(".adjustment:contains('+')");
        plusButton.MouseDown();
        plusButton.MouseDown();
        plusButton.MouseDown();

        // Act
        component.InvokeAsync(() => component.Instance.Reset());

        // Assert
        Assert.Contains("00", component.Markup);
        Assert.Equal(0, component.Instance.Points);
    }
}
