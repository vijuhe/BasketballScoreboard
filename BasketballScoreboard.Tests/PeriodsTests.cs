using Bunit;
using BasketballScoreboard.Components;

namespace BasketballScoreboard.Tests;

public class PeriodsTests : TestContext
{
    [Fact]
    public void Periods_InitialState_ShouldShowFirstPeriod()
    {
        // Arrange & Act
        var component = RenderComponent<Periods>();

        // Assert
        Assert.Contains("PERIOD", component.Markup);
        
        // Check that period 1 is selected (has red background)
        var periodButtons = component.FindAll(".clickable");
        Assert.Equal(5, periodButtons.Count); // 1, 2, 3, 4, OT
        
        // Period 1 should have red background
        Assert.Contains("background-color: red;", periodButtons[0].GetAttribute("style"));
        
        // Other periods should have no background color (empty style)
        for (int i = 1; i < 5; i++)
        {
            var style = periodButtons[i].GetAttribute("style");
            Assert.True(string.IsNullOrEmpty(style) || !style.Contains("background-color: red;"));
        }
    }

    [Fact]
    public void Periods_ClickPeriod2_ShouldSelectPeriod2()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act
        var period2Button = component.FindAll(".clickable")[1]; // Index 1 is period 2
        period2Button.MouseDown();

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // Period 2 should now have red background
        Assert.Contains("background-color: red;", periodButtons[1].GetAttribute("style"));
        
        // Other periods should not have red background
        Assert.DoesNotContain("background-color: red;", periodButtons[0].GetAttribute("style"));
        Assert.DoesNotContain("background-color: red;", periodButtons[2].GetAttribute("style"));
        Assert.DoesNotContain("background-color: red;", periodButtons[3].GetAttribute("style"));
        Assert.DoesNotContain("background-color: red;", periodButtons[4].GetAttribute("style"));
    }

    [Fact]
    public void Periods_ClickOvertime_ShouldSelectOvertime()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act
        var overtimeButton = component.FindAll(".clickable")[4]; // Index 4 is OT
        overtimeButton.MouseDown();

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // OT should now have red background
        Assert.Contains("background-color: red;", periodButtons[4].GetAttribute("style"));
        
        // Other periods should not have red background
        for (int i = 0; i < 4; i++)
        {
            Assert.DoesNotContain("background-color: red;", periodButtons[i].GetAttribute("style"));
        }
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "2")]
    [InlineData(2, "3")]
    [InlineData(3, "4")]
    [InlineData(4, "OT")]
    public void Periods_ShouldDisplayCorrectText(int buttonIndex, string expectedText)
    {
        // Arrange & Act
        var component = RenderComponent<Periods>();

        // Assert
        var periodButtons = component.FindAll(".clickable");
        Assert.Contains(expectedText, periodButtons[buttonIndex].TextContent);
    }

    [Fact]
    public void Periods_Next_ShouldMoveToNextPeriod()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act
        component.InvokeAsync(() => component.Instance.Next());

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // Period 2 should now be selected
        Assert.Contains("background-color: red;", periodButtons[1].GetAttribute("style"));
    }

    [Fact]
    public void Periods_NextMultipleTimes_ShouldProgressThroughPeriods()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act - Move through periods 1 -> 2 -> 3 -> 4
        component.InvokeAsync(() => component.Instance.Next()); // Now period 2
        component.InvokeAsync(() => component.Instance.Next()); // Now period 3
        component.InvokeAsync(() => component.Instance.Next()); // Now period 4

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // Period 4 should now be selected
        Assert.Contains("background-color: red;", periodButtons[3].GetAttribute("style"));
        
        // Should be in the last period
        Assert.True(component.Instance.IsLastPeriod);
    }

    [Fact]
    public void Periods_NextAtMaximum_ShouldNotExceedOvertime()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act - Move to overtime and try to go beyond
        component.InvokeAsync(() => component.Instance.Next()); // Period 2
        component.InvokeAsync(() => component.Instance.Next()); // Period 3
        component.InvokeAsync(() => component.Instance.Next()); // Period 4
        component.InvokeAsync(() => component.Instance.Next()); // Overtime
        component.InvokeAsync(() => component.Instance.Next()); // Should stay at overtime

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // Should still be at overtime (period 5)
        Assert.Contains("background-color: red;", periodButtons[4].GetAttribute("style"));
    }

    [Fact]
    public void Periods_Reset_ShouldReturnToFirstPeriod()
    {
        // Arrange
        var component = RenderComponent<Periods>();
        
        // Move to period 3 first
        component.InvokeAsync(() => component.Instance.Next());
        component.InvokeAsync(() => component.Instance.Next());

        // Act
        component.InvokeAsync(() => component.Instance.Reset());

        // Assert
        var periodButtons = component.FindAll(".clickable");
        
        // Period 1 should be selected again
        Assert.Contains("background-color: red;", periodButtons[0].GetAttribute("style"));
        
        // Should not be in the last period anymore
        Assert.False(component.Instance.IsLastPeriod);
    }

    [Fact]
    public void Periods_IsLastPeriod_ShouldBeTrueAtPeriod4AndOvertime()
    {
        // Arrange
        var component = RenderComponent<Periods>();

        // Act & Assert - Period 1-3 should not be last
        Assert.False(component.Instance.IsLastPeriod); // Period 1
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.False(component.Instance.IsLastPeriod); // Period 2
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.False(component.Instance.IsLastPeriod); // Period 3
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.True(component.Instance.IsLastPeriod); // Period 4
        
        component.InvokeAsync(() => component.Instance.Next());
        Assert.True(component.Instance.IsLastPeriod); // Overtime
    }

    [Fact]
    public void Periods_OnPeriodManuallyChanged_ShouldBeInvokedWhenPeriodChanges()
    {
        // Arrange
        bool eventInvoked = false;
        var component = RenderComponent<Periods>(parameters => parameters
            .Add(p => p.OnPeriodManuallyChanged, () => eventInvoked = true));

        // Act
        var period3Button = component.FindAll(".clickable")[2]; // Click period 3
        period3Button.MouseDown();

        // Assert
        Assert.True(eventInvoked);
    }

    [Fact]
    public void Periods_OnPeriodManuallyChanged_ShouldNotBeInvokedWhenClickingSamePeriod()
    {
        // Arrange
        bool eventInvoked = false;
        var component = RenderComponent<Periods>(parameters => parameters
            .Add(p => p.OnPeriodManuallyChanged, () => eventInvoked = true));

        // Act - Click period 1 (which is already selected)
        var period1Button = component.FindAll(".clickable")[0];
        period1Button.MouseDown();

        // Assert
        Assert.False(eventInvoked);
    }
}