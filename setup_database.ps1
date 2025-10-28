# PowerShell script to set up IdeaPulse database
Write-Host "🚀 Setting up IdeaPulse Database..." -ForegroundColor Green

# Step 1: Restore packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# Step 2: Build the project
Write-Host "🔨 Building project..." -ForegroundColor Yellow
dotnet build

# Step 3: Apply database migrations using Entity Framework
Write-Host "🗄️ Applying database migrations..." -ForegroundColor Yellow

# First, let's check if we need to create initial migration
if (!(Test-Path "Migrations")) {
    Write-Host "Creating initial migration..." -ForegroundColor Cyan
    dotnet ef migrations add InitialCreate
}

# Apply the migration
Write-Host "Updating database..." -ForegroundColor Cyan
dotnet ef database update

Write-Host "✅ Database setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Add your OpenAI API key to appsettings.json" -ForegroundColor White
Write-Host "2. Run 'dotnet run' to start the application" -ForegroundColor White
Write-Host "3. Visit https://localhost:5001" -ForegroundColor White
Write-Host "4. Login with admin@ideapulse.com / admin123" -ForegroundColor White

