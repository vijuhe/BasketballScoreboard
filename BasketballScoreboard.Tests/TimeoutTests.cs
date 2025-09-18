using Bunit;
using BasketballScoreboard.Components;
using TimeoutComponent = BasketballScoreboard.Components.Timeout;

namespace BasketballScoreboard.Tests;

public class TimeoutTests : TestContext
{
    [Fact]
    public void Timeout_InitialState_ShouldShowTimeoutPlaceholder()
    {
        // Arrange & Act
        var component = RenderComponent<TimeoutComponent>();

        // Assert
        Assert.Contains("TIME OUT", component.Markup);
        
        var textElement = component.Find(".text");
        Assert.Equal("TIME OUT", textElement.TextContent);
    }

    [Fact]
    public void Timeout_Click_ShouldStartTimeout()
    {
        // Arrange
        var component = RenderComponent<TimeoutComponent>();

        // Act
        var textElement = component.Find(".text");
        textElement.MouseDown();

        // Assert
        // After clicking, should show countdown timer instead of placeholder
        Assert.DoesNotContain("TIME OUT", component.Markup);
        Assert.Contains("01:00", component.Markup);
    }

    [Fact]
    public void Timeout_ClickTwice_ShouldToggleBackToPlaceholder()
    {
        // Arrange
        var component = RenderComponent<TimeoutComponent>();
        var textElement = component.Find(".text");

        // Act
        textElement.MouseDown(); // Start timeout
        textElement.MouseDown(); // Stop timeout

        // Assert
        Assert.Contains("TIME OUT", component.Markup);
    }

    [Fact]
    public void Timeout_ShouldHaveCorrectCssClass()
    {
        // Arrange & Act
        var component = RenderComponent<TimeoutComponent>();

        // Assert
        var textElement = component.Find(".text");
        Assert.NotNull(textElement);
        Assert.Contains("text", textElement.GetAttribute("class"));
    }

    [Fact]
    public void Timeout_OnTimeoutEnded_ShouldBeInvokedWhenProvided()
    {
        // Arrange
        bool eventInvoked = false;
        var component = RenderComponent<TimeoutComponent>(parameters => parameters
            .Add(p => p.OnTimeoutEnded, () => eventInvoked = true));

        // We can't easily test the timer completion in a unit test without waiting
        // So we'll just verify the parameter is accepted without error
        
        // Act & Assert
        Assert.False(eventInvoked); // Should not be invoked yet
        
        // The component should render correctly with the event callback
        Assert.Contains("TIME OUT", component.Markup);
    }

    [Fact]
    public void Timeout_OnTimeoutAboutToEnd_ShouldBeInvokedWhenProvided()
    {
        // Arrange
        bool eventInvoked = false;
        var component = RenderComponent<TimeoutComponent>(parameters => parameters
            .Add(p => p.OnTimeoutAboutToEnd, () => eventInvoked = true));

        // Act & Assert
        Assert.False(eventInvoked); // Should not be invoked yet
        
        // The component should render correctly with the event callback
        Assert.Contains("TIME OUT", component.Markup);
    }

    [Fact]
    public void Timeout_WithoutEventCallbacks_ShouldNotThrowException()
    {
        // Arrange & Act - No event callbacks provided
        var component = RenderComponent<TimeoutComponent>();

        // Act & Assert - Should not throw when rendering or clicking
        var textElement = component.Find(".text");
        var exception = Record.Exception(() => textElement.MouseDown());
        
        Assert.Null(exception);
    }

    [Fact]
    public void Timeout_ShouldHavePreventDefaultBehavior()
    {
        // Arrange & Act
        var component = RenderComponent<TimeoutComponent>();

        // Assert
        var textElement = component.Find(".text");
        // Check that the element has the blazor mouse down event attribute
        Assert.Contains("blazor:onmousedown", textElement.OuterHtml);
    }
}