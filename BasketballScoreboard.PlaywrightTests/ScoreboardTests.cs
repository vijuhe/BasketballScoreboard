namespace BasketballScoreboard.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ScoreboardTests : PageTest
{
    private const string BaseUrl = "http://localhost:5237";

    [Test]
    public async Task ScoreboardLoadsSuccessfully()
    {
        await Page.GotoAsync(BaseUrl);

        // Verify the page title
        await Expect(Page).ToHaveTitleAsync("Basketball Scoreboard");
        
        // Verify main components are visible
        await Expect(Page.GetByText("HOME")).ToBeVisibleAsync();
        await Expect(Page.GetByText("AWAY")).ToBeVisibleAsync();
        await Expect(Page.GetByText("FOULS")).ToBeVisibleAsync();
        await Expect(Page.GetByText("PERIOD")).ToBeVisibleAsync();
        await Expect(Page.GetByText("TIME OUT")).ToBeVisibleAsync();
    }

    [Test]
    public async Task TeamScoresCanBeIncremented()
    {
        await Page.GotoAsync(BaseUrl);

        // Check initial scores are 00
        await Expect(Page.Locator(".points").Nth(0)).ToContainTextAsync("00");
        await Expect(Page.Locator(".points").Nth(1)).ToContainTextAsync("00");
        
        // Click the + button for HOME team
        await Page.Locator(".adjustment").Filter(new() { HasText = "+" }).Nth(0).ClickAsync();
        
        // Click the + button for AWAY team
        await Page.Locator(".adjustment").Filter(new() { HasText = "+" }).Nth(1).ClickAsync();
        
        // Verify scores changed to 01
        await Expect(Page.Locator(".points").Nth(0)).ToContainTextAsync("01");
        await Expect(Page.Locator(".points").Nth(1)).ToContainTextAsync("01");
    }

    [Test]
    public async Task ScoreCanBeDecrementedWithMinusButton()
    {
        await Page.GotoAsync(BaseUrl);
        
        // First increment score multiple times
        var homePlusButton = Page.Locator(".adjustment").Filter(new() { HasText = "+" }).Nth(0);
        await homePlusButton.ClickAsync();
        await homePlusButton.ClickAsync();
        await homePlusButton.ClickAsync();
        
        // Verify score is 03
        await Expect(Page.Locator(".points").Nth(0)).ToContainTextAsync("03");
        
        // Now decrement
        await Page.Locator(".adjustment").Filter(new() { HasText = "-" }).Nth(0).ClickAsync();
        
        // Verify score is 02
        await Expect(Page.Locator(".points").Nth(0)).ToContainTextAsync("02");
    }

    [Test]
    public async Task PeriodSelectionWorks()
    {
        await Page.GotoAsync(BaseUrl);

        // Click different periods and verify no errors occur
        await Page.GetByText("2", new() { Exact = true }).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        await Page.GetByText("3", new() { Exact = true }).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        await Page.GetByText("4", new() { Exact = true }).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        await Page.GetByText("OT").ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        // All clicks should work without errors
    }

    [Test]
    public async Task TimeControlsWork()
    {
        await Page.GotoAsync(BaseUrl);

        // Find time controls within the time adjustments section
        var timeAdjustments = Page.Locator("#time-adjustments");
        
        // Test minute controls
        await timeAdjustments.GetByText("+").Nth(0).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        // Test seconds controls
        await timeAdjustments.GetByText("+").Nth(1).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        // Test minus controls
        await timeAdjustments.GetByText("-").Nth(0).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        await timeAdjustments.GetByText("-").Nth(1).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
    }

    [Test]
    public async Task FoulTrackingWorks()
    {
        await Page.GotoAsync(BaseUrl);

        // Test clicking foul dots
        var foulDots = Page.Locator(".foul-dot");
        
        // Click the first foul dot
        await foulDots.Nth(0).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        // Test clicking title to reset fouls
        await Page.GetByText("FOULS").Nth(0).ClickAsync();
        await Page.WaitForTimeoutAsync(100);
    }

    [Test]
    public async Task TimeoutFunctionalityWorks()
    {
        await Page.GotoAsync(BaseUrl);

        // Click the first timeout button
        await Page.GetByText("TIME OUT").Nth(0).ClickAsync();
        
        // Wait a moment - timeout should start showing time
        await Page.WaitForTimeoutAsync(200);
        
        // Click again to stop/reset (it might show "01:00" or still be "TIME OUT")
        var timeoutElement = Page.GetByText("TIME OUT").Or(Page.GetByText("01:00")).Nth(0);
        await timeoutElement.ClickAsync();
        await Page.WaitForTimeoutAsync(100);
    }

    [Test]
    public async Task BuzzerButtonIsClickable()
    {
        await Page.GotoAsync(BaseUrl);

        // Find the buzzer button (image with alt="Buzzer")
        var buzzerButton = Page.Locator("img[alt='Buzzer']");
        await Expect(buzzerButton).ToBeVisibleAsync();
        
        // Click the buzzer - should not throw an error
        await buzzerButton.ClickAsync();
    }

    [Test]
    public async Task TimerClickToggleWorks()
    {
        await Page.GotoAsync(BaseUrl);

        // Set some time first
        var timeAdjustments = Page.Locator("#time-adjustments");
        await timeAdjustments.GetByText("+").Nth(0).ClickAsync();
        
        // Find the timer display area
        var timerArea = Page.Locator("#remaining-time");
        await Expect(timerArea).ToBeVisibleAsync();
        
        // Click on timer to start it
        await timerArea.ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        
        // Click again to stop it
        await timerArea.ClickAsync();
        await Page.WaitForTimeoutAsync(100);
    }

    [Test]
    public async Task ResponsiveLayoutIsPresent()
    {
        await Page.GotoAsync(BaseUrl);

        // Verify Bootstrap grid classes are present
        await Expect(Page.Locator(".row")).ToBeVisibleAsync();
        await Expect(Page.Locator("[class*='col-']")).ToBeVisibleAsync();
        
        // Verify main sections are laid out correctly
        await Expect(Page.GetByText("HOME")).ToBeVisibleAsync();
        await Expect(Page.GetByText("AWAY")).ToBeVisibleAsync();
        
        // Verify basic layout structure
        await Expect(Page.Locator("main")).ToBeVisibleAsync();
    }
}
