# 🚀 IdeaPulse - Quick Start Guide

## ✅ Database Connection Configured!

Your Neon DB connection has been automatically configured. Now follow these steps:

## Step 1: Set Up Database Tables

You have **two options** to create the database tables:

### Option A: Using EF Core (Recommended)

```powershell
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Run migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Option B: Using SQL Migration File

If you prefer to run the SQL directly in Neon DB's SQL editor:

1. Go to your Neon DB dashboard
2. Click on "SQL Editor"
3. Open the file: `Migrations/20240101000000_InitialCreate.sql`
4. Copy and paste the SQL into the editor
5. Click "Run"

Or use the PowerShell script:
```powershell
.\setup_database.ps1
```

## Step 2: Add Your OpenAI API Key

Edit `appsettings.json` and add your OpenAI API key:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-actual-api-key-here"
  }
}
```

Get your API key from: https://platform.openai.com/api-keys

## Step 3: Run the Application

```powershell
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The app will start at:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

## Step 4: Test the Application

1. Open your browser to `https://localhost:5001`
2. You'll see the IdeaPulse home page
3. Click "Sign Up" to create an account OR
4. Use the default admin credentials:

**Admin Login:**
- Email: `admin@ideapulse.com`
- Password: `admin123`

**Demo User:**
- Email: `demo@ideapulse.com`
- Password: `demo123`

*(These users are created automatically by the database seeder)*

## 🎯 Test Your First Idea Analysis

1. Click "Analyze" in the navigation
2. Enter a startup idea:
   - **Name**: "AI Tutoring Platform"
   - **Description**: "An AI-powered personalized tutoring platform for students"
   - **Industry**: "Education"
3. Click "Analyze Idea"
4. Wait for AI analysis (takes 10-30 seconds)
5. View your results!
6. Try downloading the PDF report

## 🐛 Troubleshooting

### Database Connection Error
If you see: "Npgsql.PostgresException: Connection refused"
- Check that your connection string is correct in appsettings.json
- Verify your Neon DB database is active

### Migration Error
If migrations fail:
```powershell
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### OpenAI API Error
- Make sure you added your API key to appsettings.json
- Verify the key is valid and has credits

### Port Already in Use
Change the port in `Properties/launchSettings.json`

## ✅ Verification Checklist

- [ ] Database connection configured ✅
- [ ] Tables created (migrations applied)
- [ ] OpenAI API key added
- [ ] Application runs without errors
- [ ] Can create an account
- [ ] Can analyze an idea
- [ ] Results display correctly
- [ ] PDF downloads successfully

## 🎉 You're Ready!

Once everything is set up, you can:
- Analyze unlimited startup ideas
- Download PDF reports
- Manage ideas in your dashboard
- Access the admin panel (if logged in as admin)

**Need Help?** Check `SETUP_GUIDE.md` for detailed instructions.

