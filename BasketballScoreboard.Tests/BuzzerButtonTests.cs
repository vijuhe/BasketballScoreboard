using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class BuzzerButtonTests : TestContext
{
    [Fact]
    public void BuzzerButton_InitialState_ShouldRenderCorrectly()
    {
        // Arrange & Act
        var component = RenderComponent<BuzzerButton>();

        // Assert
        Assert.Contains("buzzer-container", component.Markup);
        Assert.Contains("buzzer.png", component.Markup);
        Assert.Contains("alt=\"Buzzer\"", component.Markup);
    }

    [Fact]
    public void BuzzerButton_Click_ShouldInvokeOnClickEvent()
    {
        // Arrange
        bool eventInvoked = false;
        var component = RenderComponent<BuzzerButton>(parameters => parameters
            .Add(p => p.OnClick, () => eventInvoked = true));

        // Act
        var buzzerContainer = component.Find("#buzzer-container");
        buzzerContainer.MouseDown();

        // Assert
        Assert.True(eventInvoked);
    }

    [Fact]
    public void BuzzerButton_MultipleClicks_ShouldInvokeEventMultipleTimes()
    {
        // Arrange
        int clickCount = 0;
        var component = RenderComponent<BuzzerButton>(parameters => parameters
            .Add(p => p.OnClick, () => clickCount++));

        // Act
        var buzzerContainer = component.Find("#buzzer-container");
        buzzerContainer.MouseDown();
        buzzerContainer.MouseDown();
        buzzerContainer.MouseDown();

        // Assert
        Assert.Equal(3, clickCount);
    }

    [Fact]
    public void BuzzerButton_ShouldHaveCorrectStructure()
    {
        // Arrange & Act
        var component = RenderComponent<BuzzerButton>();

        // Assert
        // Should have the Bootstrap grid structure
        var rows = component.FindAll(".row");
        Assert.Single(rows);

        var columns = component.FindAll("[class*='col-']");
        Assert.Equal(3, columns.Count); // col-md-5, col-md-2, col-md-5

        // Middle column should contain the buzzer
        var buzzerContainer = component.Find("#buzzer-container");
        Assert.Contains("col-md-2", buzzerContainer.GetAttribute("class"));
    }

    [Fact]
    public void BuzzerButton_Image_ShouldHaveCorrectAttributes()
    {
        // Arrange & Act
        var component = RenderComponent<BuzzerButton>();

        // Assert
        var image = component.Find("img");
        Assert.Equal("buzzer.png", image.GetAttribute("src"));
        Assert.Equal("Buzzer", image.GetAttribute("alt"));
    }

    [Fact]
    public void BuzzerButton_WithoutOnClick_ShouldNotThrowException()
    {
        // Arrange & Act - No OnClick parameter provided
        var component = RenderComponent<BuzzerButton>();

        // Act & Assert - Should not throw when clicking
        var buzzerContainer = component.Find("#buzzer-container");
        var exception = Record.Exception(() => buzzerContainer.MouseDown());
        
        Assert.Null(exception);
    }
}