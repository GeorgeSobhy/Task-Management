Task Management API

A modern ASP.NET Core Web API for managing tasks with SQL Server, Redis caching, JWT authentication, and Swagger documentation.

🚀 Features
User Authentication with JWT
Role-based Authorization
Task CRUD Operations
Redis Caching
SQL Server + Entity Framework Core
Swagger API Testing
Clean Architecture
🛠️ Run Locally
Prerequisites

Make sure you have the following installed:

.NET 8 SDK
SQL Server LocalDB or SQL Server Express
Redis Server (Docker recommended)
Visual Studio 2022 or VS Code
📦 Step 1: Set Up Infrastructure

Run Redis locally using Docker:

docker run -d -p 6379:6379 --name task-redis redis

This starts Redis on port 6379.

⚙️ Step 2: Configure Environment

Open appsettings.Development.json and update:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskMgmtDB;Trusted_Connection=True;",
    "Redis": "localhost:6379"
  }
}
🗄️ Step 3: Apply Database Migrations

Run in the project root:

dotnet ef database update

This creates the database and applies migrations.

▶️ Step 4: Run the Application
dotnet run --project TaskManagement.API

Or run directly from Visual Studio.

🌐 Swagger UI

After launch, open:

https://localhost:{PORT}/swagger

Example:

https://localhost:5001/swagger

Use Swagger to test endpoints.

📌 Verify Redis Cache

Open Redis CLI:

docker exec -it task-redis redis-cli

Then run:

MONITOR

You will see cache activity in real time.

📁 Project Structure
TaskManagement.API
TaskManagement.Application
TaskManagement.Domain
TaskManagement.Infrastructure
🔐 Authentication Flow
Register User
Login
Receive JWT Token
Use token in Swagger:
Bearer YOUR_TOKEN
🧪 Tech Stack
ASP.NET Core 8
Entity Framework Core
SQL Server
Redis
JWT Authentication
Swagger
