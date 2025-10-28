# 🚀 IdeaPulse - AI-Driven Startup Idea Validator

IdeaPulse is an AI-powered web platform that helps entrepreneurs, students, and investors instantly validate startup ideas.

## 🌟 Features

- ✨ AI-powered market validation analysis
- 📊 Detailed insights on competitors, market demand, and industry trends
- 📄 Downloadable PDF reports for investors
- 🎯 Validation scoring (0-100)
- 👥 User dashboard to track all analyzed ideas
- 🔐 Secure authentication system
- 👨‍💼 Admin dashboard for platform management
- 📈 Real-time analytics and monitoring

## 🛠️ Tech Stack

- **Frontend**: ASP.NET Razor Pages
- **Backend**: ASP.NET Core 8.0
- **Database**: PostgreSQL (Neon DB cloud)
- **AI Engine**: OpenAI GPT-4 API
- **PDF Generation**: QuestPDF
- **Styling**: Custom CSS with modern design

## 📋 Prerequisites

- .NET 8.0 SDK
- PostgreSQL database (Neon DB recommended)
- OpenAI API key
- (Optional) NewsAPI key for competitor data

## ⚙️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd IdeaPulse
```

### 2. Configure Database

Update `appsettings.json` with your PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-neon-host;Database=ideapulse_db;Username=your-username;Password=your-password"
  }
}
```

### 3. Configure API Keys

Update `appsettings.json` with your API keys:

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  },
  "NewsAPI": {
    "ApiKey": "your-newsapi-key"
  }
}
```

### 4. Run Migrations

Execute the SQL migration file to create the database schema:

```bash
psql -h your-host -U your-username -d ideapulse_db -f Migrations/20240101000000_InitialCreate.sql
```

Or use EF Core tools:

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

Navigate to `https://localhost:5001` (or the port shown in the console)

## 👤 Default Users

The application seeds with the following default users:

**Admin User:**
- Email: `admin@ideapulse.com`
- Password: `admin123`
- Role: Admin

**Demo User:**
- Email: `demo@ideapulse.com`
- Password: `demo123`
- Role: Entrepreneur

## 📁 Project Structure

```
IdeaPulse/
├── Controllers/         # MVC Controllers
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── IdeaController.cs
│   ├── AdminController.cs
│   ├── ApiController.cs
│   └── PDFController.cs
├── Models/             # Data Models
│   ├── User.cs
│   ├── IdeaAnalysis.cs
│   ├── AIRequestLog.cs
│   └── DTOs/
├── Services/           # Business Logic
│   ├── OpenAIService.cs
│   ├── PDFService.cs
│   └── AuthService.cs
├── Data/              # Data Access Layer
│   ├── ApplicationDbContext.cs
│   └── DatabaseSeeder.cs
├── Views/             # Razor Views
│   ├── Home/
│   ├── Account/
│   ├── Idea/
│   ├── Admin/
│   └── Shared/
├── wwwroot/           # Static Files
│   ├── css/
│   ├── js/
│   └── lib/
└── Migrations/        # Database Migrations
```

## 🎨 Website Pages

1. **Home Page** (`/`) - Landing page with hero section and features
2. **Idea Analyzer** (`/Idea/Analyzer`) - Input page for startup ideas
3. **Analysis Result** (`/Idea/Result/{id}`) - Display AI-generated results
4. **User Dashboard** (`/Account/Dashboard`) - Manage all analyzed ideas
5. **Admin Dashboard** (`/Admin/Dashboard`) - Platform analytics and management
6. **Login/Signup** (`/Account/Login`) - User authentication
7. **About** (`/Home/About`) - Information about the platform
8. **Contact** (`/Home/Contact`) - Contact form
9. **Legal Pages** (`/Home/Privacy`, `/Home/Terms`) - Privacy and Terms

## 🔌 API Endpoints

### POST `/api/analyze`
Analyze a startup idea using AI.

**Request:**
```json
{
  "startupName": "My Startup",
  "description": "Detailed description...",
  "industry": "Technology"
}
```

**Response:**
```json
{
  "id": 1,
  "startupName": "My Startup",
  "summary": "...",
  "validationScore": 75,
  ...
}
```

### GET `/api/ideas/{id}`
Get analysis result by ID.

### GET `/api/ideas`
Get all user's analyzed ideas.

### GET `/PDF/generate/{id}`
Generate and download PDF report for an idea.

## 🎯 Usage Examples

### As an Entrepreneur:
1. Sign up for an account
2. Navigate to "Analyze" page
3. Enter your startup idea details
4. Get instant AI-powered validation
5. Download PDF report for investors
6. Track all your ideas in dashboard

### As an Investor:
1. Create account with "Investor" role
2. Use the platform to quickly validate multiple ideas
3. Export reports for portfolio management

### As an Admin:
1. Login with admin credentials
2. View platform statistics
3. Monitor AI usage and costs
4. Manage users and content

## 🔐 Security Features

- BCrypt password hashing
- Session-based authentication
- SQL injection protection via EF Core
- XSS protection in Razor views
- HTTPS enforcement

## 📊 Admin Dashboard Features

- Total users count
- Total ideas analyzed
- Average validation scores
- Recent users and ideas
- AI request logs
- System monitoring

## 🚀 Deployment

### Deploy to Azure:
```bash
az webapp create --name ideapulse --resource-group my-resource-group --plan my-plan
```

### Deploy to AWS:
```bash
dotnet publish -c Release
# Deploy to Elastic Beanstalk or EC2
```

### Environment Variables:
Set in production:
- `DefaultConnection`
- `OpenAI:ApiKey`
- `NewsAPI:ApiKey`

## 📝 License

This project is licensed under the MIT License.