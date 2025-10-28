# Security Setup Guide

## ⚠️ IMPORTANT: Keep Credentials Safe

Your database connection strings and API keys have been removed from configuration files for security.

## How to Set Up Your Local Environment

### Option 1: User Secrets (Recommended for Development)

Run these commands to set up secure user secrets:

```bash
# Initialize user secrets
dotnet user-secrets init

# Add your database connection
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=your-host;Database=your-db;Username=your-user;Password=your-password;SSL Mode=Require"

# Add your OpenAI API key
dotnet user-secrets set "OpenAI:ApiKey" "your-openai-key"

# Optional: NewsAPI key
dotnet user-secrets set "NewsAPI:ApiKey" "your-newsapi-key"
```

### Option 2: Environment Variables (Recommended for Production)

Set environment variables:

**Windows:**
```cmd
set ConnectionStrings__DefaultConnection=Host=your-host;Database=your-db;Username=your-user;Password=your-password
set OpenAI__ApiKey=your-openai-key
```

**Linux/Mac:**
```bash
export ConnectionStrings__DefaultConnection="Host=your-host;Database=your-db;Username=your-user;Password=your-password"
export OpenAI__ApiKey="your-openai-key"
```

### Option 3: Temporary Configuration File (Not Recommended)

You can temporarily create a `appsettings.local.json` (already in .gitignore) for local development:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  },
  "OpenAI": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

## What Was Secured?

✅ Removed database connection strings from all config files  
✅ Removed OpenAI API keys  
✅ Updated .gitignore to prevent future commits  
✅ Created this security guide  

## Critical Actions Required

1. **Change your exposed credentials immediately:**
   - Change your Neon DB password
   - Regenerate your OpenAI API key
   - Monitor your accounts for unauthorized access

2. **Remove from Git history** (if credentials were already pushed):
   ```bash
   # If already pushed to GitHub, you need to:
   # 1. Change the credentials NOW
   # 2. Consider using git-filter-repo to remove from history
   # 3. Force push (this will rewrite history - communicate with team first)
   ```

3. **Set up proper secrets management:**
   - Use Azure Key Vault for production
   - Use AWS Secrets Manager for AWS deployments
   - Never commit real credentials to git

## Best Practices

- ✅ Always use environment variables or user secrets
- ✅ Never commit appsettings with real credentials
- ✅ Rotate credentials regularly
- ✅ Use different credentials for dev/staging/production
- ✅ Monitor access logs for suspicious activity
- ✅ Use .gitignore to prevent accidental commits

## For Production Deployment

When deploying to production (Azure, AWS, etc.):

1. Set environment variables in your hosting platform
2. Use platform-specific secrets management (Key Vault, Secrets Manager)
3. Never store credentials in code or configuration files
4. Use managed identities when possible

