-- Initial database schema for IdeaPulse

-- Users table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "Role" VARCHAR(50) NOT NULL DEFAULT 'Entrepreneur',
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- IdeaAnalyses table
CREATE TABLE IF NOT EXISTS "IdeaAnalyses" (
    "Id" SERIAL PRIMARY KEY,
    "StartupName" VARCHAR(200) NOT NULL,
    "Description" TEXT NOT NULL,
    "Industry" VARCHAR(100),
    "IndustryInsights" TEXT DEFAULT '{}',
    "MarketDemand" TEXT DEFAULT '{}',
    "Competitors" TEXT DEFAULT '[]',
    "TargetMarket" TEXT DEFAULT '{}',
    "Challenges" TEXT DEFAULT '[]',
    "Recommendations" TEXT DEFAULT '[]',
    "Summary" TEXT DEFAULT '',
    "ValidationScore" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- AIRequestLogs table
CREATE TABLE IF NOT EXISTS "AIRequestLogs" (
    "Id" SERIAL PRIMARY KEY,
    "Endpoint" VARCHAR(200) NOT NULL,
    "RequestData" TEXT,
    "ResponseData" TEXT,
    "UserId" INTEGER,
    "Timestamp" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "Success" BOOLEAN NOT NULL DEFAULT FALSE,
    "ErrorMessage" TEXT
);

-- Create indexes
CREATE INDEX IF NOT EXISTS idx_users_email ON "Users"("Email");
CREATE INDEX IF NOT EXISTS idx_idea_analyses_user_id ON "IdeaAnalyses"("UserId");
CREATE INDEX IF NOT EXISTS idx_idea_analyses_created_at ON "IdeaAnalyses"("CreatedAt");
CREATE INDEX IF NOT EXISTS idx_ai_request_logs_timestamp ON "AIRequestLogs"("Timestamp");

