using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class TeamFoulsTests : TestContext
{
    [Fact]
    public void TeamFouls_InitialState_ShouldHaveZeroFouls()
    {
        // Arrange & Act
        var component = RenderComponent<TeamFouls>();

        // Assert
        Assert.Contains("FOULS", component.Markup);
        
        // All foul dots should be correct for zero fouls state
        var foulDots = component.FindAll(".foul-dot");
        Assert.Equal(5, foulDots.Count);
        
        // 5th foul position (dot 0) should be darkred when no fouls
        Assert.Contains("background-color: darkred;", foulDots[0].GetAttribute("style"));
        
        // Other dots should be grey
        for (int i = 1; i < 5; i++)
        {
            Assert.Contains("background-color: grey;", foulDots[i].GetAttribute("style"));
        }
    }

    [Fact]
    public void TeamFouls_ClickFirstFoul_ShouldSetToOneFoul()
    {
        // Arrange
        var component = RenderComponent<TeamFouls>();

        // Act - Click the smallest dot (position 1, index 4)
        var firstFoulDot = component.FindAll(".foul-dot")[4]; // Index 4 is the smallest (position 1)
        firstFoulDot.MouseDown();

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        
        // First dot (smallest) should be white (active)
        Assert.Contains("background-color: white;", foulDots[4].GetAttribute("style"));
        
        // 5th foul position should still be darkred (not reached)
        Assert.Contains("background-color: darkred;", foulDots[0].GetAttribute("style"));
        
        // Other dots should be grey
        for (int i = 1; i < 4; i++)
        {
            Assert.Contains("background-color: grey;", foulDots[i].GetAttribute("style"));
        }
    }

    [Fact]
    public void TeamFouls_ClickThirdFoul_ShouldSetToThreeFouls()
    {
        // Arrange
        var component = RenderComponent<TeamFouls>();

        // Act - Click the third foul dot (index 2)
        var thirdFoulDot = component.FindAll(".foul-dot")[2];
        thirdFoulDot.MouseDown();

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        
        // Dots 2, 3, 4 should be white (positions 3, 2, 1 are active)
        Assert.Contains("background-color: white;", foulDots[2].GetAttribute("style")); // 3rd foul
        Assert.Contains("background-color: white;", foulDots[3].GetAttribute("style")); // 2nd foul
        Assert.Contains("background-color: white;", foulDots[4].GetAttribute("style")); // 1st foul
        
        // 5th foul position should still be darkred (not reached)
        Assert.Contains("background-color: darkred;", foulDots[0].GetAttribute("style"));
        
        // 4th foul position should be grey (not reached yet)
        Assert.Contains("background-color: grey;", foulDots[1].GetAttribute("style"));
    }

    [Fact]
    public void TeamFouls_ClickFifthFoul_ShouldSetToFiveFoulsWithRedBackground()
    {
        // Arrange
        var component = RenderComponent<TeamFouls>();

        // Act - Click the fifth foul dot (largest one, index 0)
        var fifthFoulDot = component.FindAll(".foul-dot")[0];
        fifthFoulDot.MouseDown();

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        
        // Fifth foul dot should be red (penalty situation)
        Assert.Contains("background-color: red;", foulDots[0].GetAttribute("style"));
        
        // All other dots should be white (active)
        for (int i = 1; i < 5; i++)
        {
            Assert.Contains("background-color: white;", foulDots[i].GetAttribute("style"));
        }
    }

    [Fact]
    public void TeamFouls_ClickTitle_ShouldResetToZeroFouls()
    {
        // Arrange
        var component = RenderComponent<TeamFouls>();
        
        // First set some fouls
        var thirdFoulDot = component.FindAll(".foul-dot")[2];
        thirdFoulDot.MouseDown();

        // Act - Click the title to reset
        var title = component.Find(".title");
        title.MouseDown();

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        
        // All dots should be grey again (no fouls)
        foreach (var dot in foulDots)
        {
            var style = dot.GetAttribute("style");
            Assert.True(
                style.Contains("background-color: grey;") || style.Contains("background-color: darkred;"),
                $"Expected grey or darkred background, but got: {style}"
            );
        }
    }

    [Fact]
    public void TeamFouls_Reset_ShouldReturnToZeroFouls()
    {
        // Arrange
        var component = RenderComponent<TeamFouls>();
        
        // First set some fouls
        var fifthFoulDot = component.FindAll(".foul-dot")[0];
        fifthFoulDot.MouseDown();

        // Act
        component.InvokeAsync(() => component.Instance.Reset());

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        
        // All dots should be grey or darkred (no fouls)
        foreach (var dot in foulDots)
        {
            var style = dot.GetAttribute("style");
            Assert.True(
                style.Contains("background-color: grey;") || style.Contains("background-color: darkred;"),
                $"Expected grey or darkred background, but got: {style}"
            );
        }
    }

    [Theory]
    [InlineData(0, "background-color: darkred;")] // 5th foul position when no fouls
    [InlineData(1, "background-color: grey;")] // 4th foul position when no fouls
    [InlineData(2, "background-color: grey;")] // 3rd foul position when no fouls
    [InlineData(3, "background-color: grey;")] // 2nd foul position when no fouls
    [InlineData(4, "background-color: grey;")] // 1st foul position when no fouls
    public void TeamFouls_InitialState_ShouldHaveCorrectColors(int dotIndex, string expectedColor)
    {
        // Arrange & Act
        var component = RenderComponent<TeamFouls>();

        // Assert
        var foulDots = component.FindAll(".foul-dot");
        Assert.Contains(expectedColor, foulDots[dotIndex].GetAttribute("style"));
    }
}