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

        await Expect(Page).ToHaveTitleAsync("Basketball Scoreboard");
        await Expect(Page.GetByText("HOME")).ToBeVisibleAsync();
        await Expect(Page.GetByText("AWAY")).ToBeVisibleAsync();
        await Expect(Page.GetByText("PERIOD")).ToBeVisibleAsync();
        await Expect(Page.GetByText("MINUTES")).ToBeVisibleAsync();
        await Expect(Page.GetByText("SECONDS")).ToBeVisibleAsync();
        await Expect(Page.GetByText("FOULS")).ToHaveCountAsync(2);
        await Expect(Page.GetByText("TIME OUT")).ToHaveCountAsync(2);
    }
}
