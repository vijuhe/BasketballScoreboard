# Basketball Scoreboard - Playwright Tests

This directory contains end-to-end (E2E) tests for the Basketball Scoreboard application using Microsoft Playwright framework.

## Overview

The Playwright tests validate the complete user experience of the Basketball Scoreboard application by automating browser interactions and verifying the UI behavior in a real browser environment.

## Test Coverage

The test suite covers all major functionality of the Basketball Scoreboard:

### Core Functionality Tests
- **Application Loading**: Verifies the scoreboard loads correctly with all components visible
- **Team Scoring**: Tests score increment/decrement for both HOME and AWAY teams
- **Period Management**: Validates period selection (1-4 plus overtime)
- **Timer Controls**: Tests minute/second adjustment controls
- **Foul Tracking**: Verifies foul dot interactions and reset functionality
- **Timeout Management**: Tests timeout start/stop functionality
- **Buzzer Button**: Validates buzzer interaction
- **Timer Toggle**: Tests timer start/pause via click
- **Responsive Layout**: Ensures proper Bootstrap grid layout

### Test Structure
- **10 comprehensive test methods** covering all interactive elements
- **Focused assertions** that verify specific UI state changes
- **Robust element selection** using CSS selectors and text content
- **Wait strategies** to handle asynchronous UI updates

## Prerequisites

### .NET Requirements
- .NET 10.0 SDK
- ASP.NET Core runtime

### Playwright Setup
```bash
# Build the test project
dotnet build BasketballScoreboard.PlaywrightTests

# Install Playwright browsers (required first time)
pwsh BasketballScoreboard.PlaywrightTests/bin/Debug/net9.0/playwright.ps1 install
```

## Running the Tests

### 1. Start the Basketball Scoreboard Application
```bash
cd BasketballScoreboard
dotnet run
```
The application will start on http://localhost:5237

### 2. Run Playwright Tests
In a separate terminal:
```bash
# Run all E2E tests
dotnet test BasketballScoreboard.PlaywrightTests

# Run with detailed output
dotnet test BasketballScoreboard.PlaywrightTests --logger "console;verbosity=detailed"

# Run specific test
dotnet test BasketballScoreboard.PlaywrightTests --filter "ScoreboardLoadsSuccessfully"
```

### 3. Run Tests in Different Browsers
```bash
# Default: Chromium
dotnet test BasketballScoreboard.PlaywrightTests

# Firefox
BROWSER=firefox dotnet test BasketballScoreboard.PlaywrightTests

# WebKit (Safari-like)
BROWSER=webkit dotnet test BasketballScoreboard.PlaywrightTests
```

## Test Configuration

### Default Settings
- **Target URL**: `http://localhost:5237`
- **Browser**: Chromium (headless)
- **Timeout**: 30 seconds per test
- **Parallel Execution**: Enabled (each test runs in isolation)

### Customization
Tests can be configured via environment variables or by modifying the `BaseUrl` constant in `ScoreboardTests.cs`.

## Debugging Tests

### Visual Debugging
```bash
# Run tests in headed mode (visible browser)
HEADED=1 dotnet test BasketballScoreboard.PlaywrightTests

# Run tests in debug mode with slower execution
DEBUG=1 dotnet test BasketballScoreboard.PlaywrightTests
```

### Test Artifacts
Failed tests automatically generate:
- Screenshots of the failure state
- Video recordings (if enabled)
- Browser console logs
- Network request logs

## Integration with CI/CD

### GitHub Actions
The tests can be integrated into GitHub Actions workflows:

```yaml
- name: Run E2E Tests
  run: |
    cd BasketballScoreboard
    dotnet run &
    sleep 5
    dotnet test BasketballScoreboard.PlaywrightTests --logger trx
```

### Azure DevOps
Compatible with Azure DevOps pipelines using the standard .NET test task.

## Test Scenarios Validated

1. **Initial Load**
   - Page title verification
   - All UI components render correctly
   - Default values are set properly

2. **Score Management**
   - Home team score increment/decrement
   - Away team score increment/decrement
   - Score boundary validation (minimum 0)

3. **Game Controls**
   - Period selection and visual feedback
   - Timer minute/second adjustments
   - Timer start/pause functionality

4. **Team Management**
   - Foul tracking (visual state changes)
   - Timeout initiation and management
   - Reset functionality

5. **User Interactions**
   - Button clicks and visual feedback
   - Audio control activation (buzzer)
   - Responsive layout verification

## Architecture

### Test Organization
- **Single test class**: `ScoreboardTests` containing all E2E scenarios
- **Page Object Pattern**: Could be implemented for larger test suites
- **Fluent Assertions**: Using Playwright's expect API for readable assertions

### Element Selection Strategy
- **CSS Selectors**: Primary method for reliable element targeting
- **Text Content**: Secondary method for user-visible element validation
- **Nth() Selectors**: For handling multiple similar elements (team sections)

### Wait Strategies
- **Auto-waiting**: Playwright automatically waits for elements to be actionable
- **Explicit waits**: `WaitForTimeoutAsync()` for UI state changes
- **Assertion waits**: `ToBeVisibleAsync()` waits for element visibility

## Maintenance

### Updating Tests
When UI changes are made to the Basketball Scoreboard:
1. Update element selectors if CSS classes or structure changes
2. Modify assertions if expected text or behavior changes
3. Add new tests for new functionality

### Test Reliability
- Tests use stable selectors (CSS classes rather than generated IDs)
- Assertions focus on user-visible outcomes
- Minimal use of fixed timeouts to reduce flakiness

## Troubleshooting

### Common Issues
1. **Application not running**: Ensure the Basketball Scoreboard is started on port 5237
2. **Browser not installed**: Run the Playwright browser install command
3. **Timeout errors**: Check if the application is responsive and increase timeouts if needed
4. **Element not found**: Verify selectors match the current HTML structure

### Performance Tips
- Run tests in headless mode for faster execution
- Use parallel execution for test suites
- Optimize wait times based on application responsiveness