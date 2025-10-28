# IdeaPulse Setup Guide

## Quick Start

Follow these steps to get IdeaPulse running on your local machine.

## Step 1: Install Prerequisites

### .NET 8.0 SDK
Download and install from: https://dotnet.microsoft.com/download/dotnet/8.0

Verify installation:
```bash
dotnet --version
```

### PostgreSQL Database
**Option A: Local PostgreSQL**
- Install PostgreSQL locally
- Create a database named `ideapulse_db`

**Option B: Neon DB (Cloud PostgreSQL)**
- Sign up at https://neon.tech
- Create a new project
- Copy your connection string

## Step 2: Get API Keys

### OpenAI API Key
1. Sign up at https://platform.openai.com
2. Navigate to API Keys section
3. Create a new secret key
4. Copy the key (starts with `sk-`)

### NewsAPI Key (Optional)
1. Sign up at https://newsapi.org
2. Get your free API key
3. Copy the key

## Step 3: Configure the Application

### Update Database Connection

Edit `appsettings.Development.json` (or create `appsettings.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-host;Database=ideapulse_db;Username=your-user;Password=your-password"
  }
}
```

### Update API Keys

Edit the same file to add your keys:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-openai-key"
  },
  "NewsAPI": {
    "ApiKey": "your-newsapi-key"
  }
}
```

## Step 4: Set Up Database

### Option A: Using PostgreSQL CLI

```bash
# Connect to your database
psql -h your-host -U your-user -d ideapulse_db

# Run the migration
\i Migrations/20240101000000_InitialCreate.sql
```

### Option B: Using EF Core Migrations

```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Run migrations
dotnet ef database update
```

## Step 5: Run the Application

```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will start on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## Step 6: Access the Application

Open your browser and navigate to:
```
https://localhost:5001
```

## Default Login Credentials

**Admin User:**
- Email: `admin@ideapulse.com`
- Password: `admin123`
- Access: Full admin dashboard

**Demo User:**
- Email: `demo@ideapulse.com`
- Password: `demo123`
- Access: User dashboard

## Troubleshooting

### Database Connection Errors

**Error:** `Npgsql.PostgresException: Connection refused`

**Solution:** Verify your PostgreSQL connection string in `appsettings.json`

### OpenAI API Errors

**Error:** `OpenAI API error: 401 Unauthorized`

**Solution:** Check that your OpenAI API key is correct and has credits

### Missing Dependencies

**Error:** `Package not found`

**Solution:** Run `dotnet restore` to install all NuGet packages

### Port Already in Use

**Error:** `Failed to bind to address`

**Solution:** Change the port in `Properties/launchSettings.json`

## Development Tips

### Hot Reload
Enable hot reload for faster development:
```bash
dotnet watch run
```

### View Database
Use pgAdmin or DBeaver to view your database:
1. Connect to your PostgreSQL instance
2. Navigate to the `ideapulse_db` database
3. Explore tables: Users, IdeaAnalyses, AIRequestLogs

### Test OpenAI Integration
Create a test idea to verify OpenAI integration:
1. Login as demo user
2. Go to "Analyze" page
3. Enter any startup idea
4. Wait for AI analysis (takes ~10-30 seconds)

## Next Steps

1. ✅ Configure database connection
2. ✅ Add OpenAI API key
3. ✅ Run the application
4. ✅ Test idea analysis
5. ✅ Explore admin dashboard
6. ✅ Customize as needed

## Production Deployment

When deploying to production:

1. Set environment variables on your host
2. Use secure connection strings
3. Enable HTTPS
4. Configure CORS if needed
5. Set up logging and monitoring
6. Use secrets management

## Support

If you encounter issues:
1. Check the logs in console output
2. Review database connection
3. Verify API keys are valid
4. Check database schema

For detailed documentation, see `README.md`

