# Task-Management

## How to Run Locally
Prerequisites
.NET 8.0 SDK (or the version specified in your .csproj).

SQL Server (LocalDB or Express).

Redis Server (via Docker or local installation).

Visual Studio 2022 or VS Code.

Step 1: Set Up Infrastructure
The easiest way to get the supporting services running is via Docker:

Bash
# Start Redis
docker run -d -p 6379:6379 --name task-redis redis
Step 2: Configure Environment
Open appsettings.Development.json and ensure your connection strings are correct:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskMgmtDB;Trusted_Connection=True;",
    "Redis": "localhost:6379"
  }
}
Step 3: Apply Migrations
Open your terminal in the project root and run:

Bash
dotnet ef database update
Step 4: Launch the Application
You can run the project directly from your IDE or via the CLI:

Bash
dotnet run --project TaskManagement.API
Swagger UI: Once running, navigate to https://localhost:PORT/swagger to test the endpoints.

Verify Redis: You can use redis-cli and run the command MONITOR to see task data being cached in real-time.
