# Basketball Scoreboard - Copilot Instructions

Basketball Scoreboard is a Blazor WebAssembly application built with .NET 9.0. It runs entirely in the browser and requires no internet connectivity once loaded. The application provides a full-featured basketball scoreboard interface designed for larger screens (laptops and external displays).

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Setup
- Install .NET 9.0 SDK:
  ```bash
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.102
  export PATH="$HOME/.dotnet:$PATH"
  ```
- Verify installation: `dotnet --version` should return `9.0.102`

### Build and Development Commands
- **Restore dependencies**: `dotnet restore` -- takes ~7 seconds. NEVER CANCEL. Set timeout to 30+ seconds.
- **Build the application**: `dotnet build` -- takes ~8 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
- **Run the development server**: `dotnet run` -- application starts in ~3 seconds and runs on http://localhost:5237
- **Format code**: `dotnet format` -- takes ~8 seconds. Always run before committing changes.
- **Verify formatting**: `dotnet format --verify-no-changes` -- validates code formatting without making changes

### Build Directory Structure
Navigate to the project directory before running any commands:
```bash
cd /home/runner/work/BasketballScoreboard/BasketballScoreboard
```

## Application Architecture

### Project Structure
- **Solution file**: `BasketballScoreboard.sln` (root level)
- **Main project**: `BasketballScoreboard/BasketballScoreboard.csproj` (.NET 9.0 Blazor WebAssembly)
- **Key components**: `BasketballScoreboard/Components/` contains all scoreboard UI components
- **Pages**: `BasketballScoreboard/Pages/Index.razor` is the main scoreboard page
- **Static assets**: `BasketballScoreboard/wwwroot/` contains CSS, images, and static content

### Key Components
- `Buzzer.razor` - Sound/buzzer functionality
- `TeamScore.razor` - Team scoring display and controls
- `RemainingTime.razor` - Game timer functionality
- `Periods.razor` - Period/quarter tracking
- `TeamFouls.razor` - Foul tracking for each team
- `Timeout.razor` - Timeout management

## Validation and Testing

### Manual Validation Scenarios
After making any changes, ALWAYS test these scenarios:

1. **Basic Application Loading**:
   - Run `dotnet run` and navigate to http://localhost:5237
   - Verify the scoreboard loads completely (shows HOME/AWAY teams, scores, timer, fouls)
   - Check browser console for any errors

2. **Interactive Functionality Testing**:
   - Click the HOME team "+" button to increment score - verify score changes from "00" to "01"
   - Click the AWAY team "+" button to increment score
   - Click the buzzer button in the center - verify it's clickable
   - Test period buttons (1, 2, 3, 4, OT) - verify selection changes
   - Test timeout buttons for both teams
   - Test time controls (MINUTES +/- and SECONDS +/-)

3. **Deployment Validation**:
   - Build output is generated in `bin/Debug/net9.0/wwwroot/`
   - Static files are properly generated for Azure Static Web Apps deployment

### Development Server
- **URL**: http://localhost:5237 (configured in `Properties/launchSettings.json`)
- **Environment**: Development mode with hot reload
- **Browser compatibility**: Modern browsers supporting WebAssembly

## CI/CD Integration

### Azure Static Web Apps
The repository uses Azure Static Web Apps for deployment:
- **Workflow files**: `.github/workflows/azure-static-web-apps-*.yml`
- **App location**: `BasketballScoreboard/` (project directory)
- **Output location**: `wwwroot` (build output directory)
- **Build process**: Automatic via Azure Static Web Apps action

### Code Quality
- **ALWAYS run `dotnet format` before committing** - the CI will fail if code is not properly formatted
- The build produces 1 warning about an unawaited async call in `Index.razor` line 124 - this is expected
- No unit tests are present in this project

## Troubleshooting

### Common Issues and Solutions

1. **Build fails with "does not support targeting .NET 9.0"**:
   - Install .NET 9.0 SDK using the command in Prerequisites section
   - Verify with `dotnet --version`

2. **Application won't start or shows loading screen indefinitely**:
   - Check browser console for WebAssembly loading errors
   - Verify all static assets are being served correctly
   - Ensure the application built successfully without errors

3. **Format command fails**:
   - Run `dotnet restore` first to ensure all packages are available
   - Check for syntax errors in Razor components

4. **Browser compatibility issues**:
   - This is a WebAssembly application - requires modern browsers
   - Mobile devices are not supported (UI designed for larger screens)

## Performance Notes
- **First load**: WebAssembly takes a few seconds to download and initialize (~14MB resources)
- **Subsequent interactions**: All functionality is client-side and should be immediate
- **Memory usage**: Typical for Blazor WebAssembly applications
- **No server dependencies**: Fully client-side application after initial load

## File Locations Reference

### Frequently Modified Files
- Main scoreboard logic: `BasketballScoreboard/Pages/Index.razor`
- Component styling: `BasketballScoreboard/Components/*.razor.css`
- Static assets: `BasketballScoreboard/wwwroot/`

### Configuration Files
- Project configuration: `BasketballScoreboard/BasketballScoreboard.csproj`
- Launch settings: `BasketballScoreboard/Properties/launchSettings.json`
- Git ignore: `.gitignore` (comprehensive .NET/Visual Studio template)

### Build Output
- Debug build: `BasketballScoreboard/bin/Debug/net9.0/`
- Published assets: `BasketballScoreboard/bin/Debug/net9.0/wwwroot/`

## Quick Command Reference

```bash
# Setup (run once)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.102
export PATH="$HOME/.dotnet:$PATH"

# Development workflow
cd /home/runner/work/BasketballScoreboard/BasketballScoreboard
dotnet restore    # ~7s
dotnet build      # ~8s  
dotnet run        # Starts dev server on http://localhost:5237

# Before committing
dotnet format     # ~8s - Format code
dotnet format --verify-no-changes  # Verify formatting
```

## Common Repository Contents

### Repository root structure
```
.
├── .github/workflows/          # Azure Static Web Apps CI/CD
├── .gitignore                  # .NET/VS template
├── BasketballScoreboard/       # Main project directory
├── BasketballScoreboard.sln    # Solution file
└── README.md                   # Basic project description
```

### Project directory contents
```
BasketballScoreboard/
├── Components/                 # Scoreboard UI components
├── Pages/                      # Main application pages
├── Properties/                 # Launch settings
├── Shared/                     # Shared components
├── wwwroot/                    # Static web assets
├── App.razor                   # App root component
├── Program.cs                  # Application entry point
└── BasketballScoreboard.csproj # Project file
```