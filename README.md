# TaskManager - Modern Task and Project Management System

## ✨ Features

✅ **User Authentication** - Secure JWT-based login with refresh tokens
📁 **Project Management** - Create, manage, and organize projects
📋 **Task Tracking** - Create, assign, prioritize, and track tasks
<!-- 📅 **Deadline Management** - Set and monitor due dates -->
👥 **Team Collaboration** - Add team members to projects
🔍 **Status Tracking** - Track task and project statuses
🔒 **Data Storage** - PostgreSQL database integration
📊 **API Documentation** - Swagger UI for easy API exploration

---

## 🛠️ Tech Stack

**Core Technologies:**
- **Language:** C# (.NET 10)
- **Backend:** ASP.NET Core Web API
- **Database:** PostgreSQL (via Npgsql)
- **Authentication:** JWT (JSON Web Tokens)
- **ORM:** Entity Framework Core

**Additional Libraries:**
- BCrypt.Net-Next (Password hashing)
- Swashbuckle.AspNetCore (Swagger documentation)
- Humanizer (String manipulation)
- Microsoft.IdentityModel (JWT handling)

**Development Tools:**
- Visual Studio Code
- Postman


<!-- - .NET CLI -->
<!-- - Docker (optional) -->

---

## 📦 Installation

### Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (or Supabase)
- [Git](https://git-scm.com/downloads)

### Quick Start

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Ldn26/TaskManager.git
   cd TaskManager
   ```

2. **Set up environment variables:**
   Create a `.env` file in the root directory with your configuration:
   ```env
   Jwt__Key=your-strong-secret-key-here
   ConnectionStrings__Supabase=Host=your-db-host;Port=5432;Username=your-username;Password=your-password;Database=your-db;SSL Mode=Require
   ```
<!-- 
3. **Restore dependencies:**
   ```bash
   dotnet restore
   ``` -->

3. **Apply database migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Run the application:**
   ```bash
   dotnet run --project TaskManager.API
   ```
      ```bash
   dotnet watch  run --project TaskManager.API    
   ```

6. **Access the API:**
   - API: `https://localhost:5260/index.html`
   - Frontend : `http://localhost:3000`

---

## 🎯 Usage

### Basic API Endpoints

#### Authentication
```csharp
// Register a new user
POST /api/users/register
{
    "email": "user@example.com",
    "password": "securePassword123",
    "fullName": "John Doe",
    "role": "Member"
}

// Login
POST /api/users/login
{
    "email": "user@example.com",
    "password": "securePassword123"
}
```

#### Project Management
```csharp
// Create a new project
POST /api/projects
{
    "name": "Website Redesign",
    "status": "Active",
    "memberIds": ["user1-id", "user2-id"]
}

// Get all projects
GET /api/projects
```

#### Task Management
```csharp
// Create a new task
POST /api/tasks
{
    "title": "Design homepage",
    "status": "Todo",
    "priority": "High",
    "dueDate": "2023-12-31T23:59:59",
    "projectId": "project1-id",
    "assignedUserId": "user1-id"
}

// Get all tasks
GET /api/tasks
```

---

## 📁 Project Structure

```
TaskManager/
├── TaskManager.API/          # API project
│   ├── Controllers/          # API endpoints
│   ├── Models/               # DTOs and models
│   ├── Services/             # Business logic
│   ├── Program.cs            # Application entry point
│   └── appsettings.json      # Configuration   
├── TaskManager.Application/  # Application layer
│   ├── DTO/                  # Data Transfer Objects
│   └── Interfaces/           # Service interfaces
├── TaskManager.Domain/       # Domain layer
│   ├── Entities/             # Domain models
│   ├── Enums/                # Enumerations
│   └── ValueObjects/         # Value objects
├── TaskManager.Infrastructure/ # Infrastructure layer
│   ├── DBConn.cs             # Database context
│   ├── Migrations/           # Database migrations
│   └── Services/             # Implementation of interfaces
├── .gitignore                # Git ignore rules
└── README.md                 # This file
```

---

## 🔧 Configuration

### Environment Variables

| Variable | Description | Example Value |
|----------|-------------|---------------|
| `Jwt__Key` | JWT secret key | `your-strong-secret-key-here` |
<!-- | `Jwt__Issuer` | JWT issuer | `your-app` | -->
<!-- | `Jwt__Audience` | JWT audience | `your-app-users` | -->
| `ConnectionStrings__Supabase` | Database connection string | `Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=taskmanager` |

### Database Configuration

The application uses Entity Framework Core with PostgreSQL. The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Supabase": "Host=your-db-host;Port=5432;Username=your-username;Password=your-password;Database=your-db;SSL Mode=Require"
  }
}
```

---

## 🤝 Contributing

We welcome contributions from the community! Here's how you can contribute:

### Development Setup

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Style Guidelines

- Follow the existing code style and formatting
- Use consistent naming conventions
- Write clear, concise comments
- Include unit tests for new features
- Keep pull requests focused on a single feature

### Pull Request Process

1. Ensure all tests pass
2. Update the documentation if necessary
3. Submit a clear description of your changes
4. Reference any related issues

---

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors & Contributors

**Maintainer:**
- [@Ldn26](https://github.com/Ldn26)

<!-- **Contributors:**
- [@contributor1](https://github.com/contributor1)
- [@contributor2](https://github.com/contributor2) -->

---

## 🐛 Issues & Support

### Reporting Issues

If you encounter any problems or have feature requests, please:

1. Check if an issue already exists
2. Create a new issue with:
   - Clear description of the problem
   - Steps to reproduce
   - Expected behavior
   - Your environment details

### Getting Help

- Open an issue on GitHub
- Join our [Discord community](https://discord.gg/your-invite-link)
- Check our [FAQ](docs/FAQ.md)

---

## 🗺️ Roadmap

### Planned Features

- [ ] User profile management
- [ ] Task comments and attachments
- [ ] Project templates
- [ ] Advanced reporting and analytics
- [ ] Mobile application (React Native)
- [ ] Webhooks for external integrations

### Known Issues

- [#123](https://github.com/Ldn26/TaskManager/issues/123) - JWT token refresh implementation
- [#456](https://github.com/Ldn26/TaskManager/issues/456) - Database connection pooling optimization

### Future Improvements

- Add GraphQL support
- Implement caching layer
- Add more detailed audit logging
- Improve performance metrics

---

## 🚀 Getting Started with Development

### Running Tests

```bash
dotnet test TaskManager.Tests
```

### Debugging

1. Set breakpoints in Visual Studio or Rider
2. Use the built-in debugging tools
3. Check the console output for errors

### Building for Production

```bash
dotnet publish -c Release -o ./publish
```

---

## 📊 Performance Considerations

1. **Database Optimization:**
   - Use proper indexing for frequently queried columns
   - Implement pagination for list endpoints
   - Consider read replicas for high-traffic applications

2. **API Performance:**
   - Implement caching for frequently accessed data
   - Use response compression
   - Consider rate limiting

3. **Memory Management:**
   - Be mindful of large collections in memory
   - Implement proper disposal of resources

---

## 🔄 Deployment

### Docker Deployment

1. Build the Docker image:
   ```bash
   docker build -t taskmanager-api .
   ```

2. Run the container:
   ```bash
   docker run -d -p 80:80 -p 443:443 --env-file .env taskmanager-api
   ```

### Kubernetes Deployment

For production deployments, consider using Kubernetes with:

- Horizontal Pod Autoscaler
- Persistent Volume Claims for database
- Ingress controller for routing

---

## 📚 Learning Resources

- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [JWT Authentication in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
- [PostgreSQL Best Practices](https://www.postgresql.org/docs/current/best-practices.html)

---




## 💡 Pro Tips

1. **Database Maintenance:**
   ```bash
   # Regularly vacuum your database
   vacuum analyze taskmanager_db

   # Consider adding indexes for performance
   CREATE INDEX idx_tasks_project_id ON tasks(project_id);
   ```

2. **API Best Practices:**
   ```csharp
   // Always validate input data
   if (string.IsNullOrWhiteSpace(dto.Name))
       return BadRequest("Project name is required");

   // Use proper HTTP status codes
   if (!projectExists)
       return NotFound("Project not found");

   // Implement proper error handling
   try
   {
       // Database operations
   }
   catch (DbUpdateException ex)
   {
       return StatusCode(500, "Database error occurred");
   }
   ```

3. **Security:**
   - Always use HTTPS in production
   - Implement proper CORS policies
   - Regularly rotate your JWT secret keys

---

## 📌 Important Notes

1. **Database Schema Changes:**
   - Always create new migrations for schema changes
   - Test migrations thoroughly before production deployment

2. **Environment Configuration:**
   - Never commit sensitive configuration files
   - Use environment-specific configuration files

3. **Versioning:**
   - Follow semantic versioning for API endpoints
   - Document breaking changes clearly

---

---


---

---

