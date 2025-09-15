# Popsicle Factory API

A comprehensive RESTful web service for managing Popsicle inventory at Popsicle Co., built with ASP.NET Core 8

## Table of Contents

- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Development Guide](#development-guide)
- [Testing](#testing)
- [Deployment](#deployment)
- [Scripts Reference](#scripts-reference)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/) (optional, for version control)
- [Postman](https://www.postman.com/) (optional, for API testing)

### Development Tools (Recommended)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Swagger UI](https://swagger.io/) (included in project)

## Quick Start

### 1. Clone and Setup
```bash
git clone <repository-url>
cd PopsicleFactory
chmod +x *.sh
./setup.sh
```

### 2. Run the API
```bash
./run.sh
# OR for development with hot reload
./run.sh dev
```

### 3. Access the API
- **API Base URL**: https://localhost:7212
- **Health Check**: https://localhost:7212/health

## Architecture Overview

This project implements a layered architecture following SOLID principles:

```
┌─────────────────────────────────────────┐
│              Controllers                │  ← HTTP handling, routing
├─────────────────────────────────────────┤
│               Services                  │  ← Business logic
├─────────────────────────────────────────┤
│             Repositories                │  ← Data access
├─────────────────────────────────────────┤
│               Models                    │  ← Data structures
└─────────────────────────────────────────┘
```

### SOLID Principles Implementation

- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Extensible through interfaces, closed for modification
- **Liskov Substitution**: Interfaces are substitutable
- **Interface Segregation**: Focused, specific interfaces
- **Dependency Inversion**: Dependencies on abstractions, not concretions

## Project Structure

```
PopsicleFactory/
├── 📁 API/                                 # Main Web API Project
│   ├── 📁 Controllers/
│   │   └── PopsicleController.cs           # REST API endpoints
│   ├── 📁 Models/
│   │   ├── Popsicle.cs                     # Core domain model
│   │   ├── 📁 DTOs/                   
│   │   ├──  📁 PopsicleDTOs/
    │   │      ├── CreatePopsicleDto.cs        # Creation request model
    │   │      ├── UpdatePopsicleDto.cs        # Partial update model
    │   │      ├── PopsicleViewModel.cs        # Response model
    │   │      └── PopsicleSearchDto.cs        # Search criteria model
│   │   └── 📁 Responses/
│   │       ├── ErrorResponse.cs            # Error response models
│   │       ├── ValidationErrorResponse.cs
│   │       └── ApiResponse.cs
│   ├── 📁 Services/
│   │   ├── 📁 Interfaces/
│   │   │   └── IPopsicleService.cs         # Service contract
│   │   └── PopsicleService.cs              # Business logic implementation
│   ├── 📁 Repositories/
│   │   ├── 📁 Interfaces/
│   │   │   └── IPopsicleRepository.cs      # Repository contract
│   │   └── PopsicleRepository.cs           # Data access implementation
│   ├── 📁 Middleware/
│   │   └── GlobalExceptionHandlingMiddleware.cs
│   ├── Program.cs                          # Application entry point
├── 📁 API.Tests/                          # Unit Test Project
│   ├── PopsicleControllerTests.cs          # Controller tests
│   ├── PopsicleServiceTests.cs             # Service layer tests
│   ├── PopsicleRepositoryTests.cs          # Repository tests
│   └── API.Tests.csproj                    # Test project configuration
├── 📁 Solution Items/                     # Solution-level files
│   ├── 📁 scripts/
│   │   ├── setup.sh                        # Project setup script
│   │   ├── run.sh                          # Development server script
│   │   ├── run.ps1                         # Windows PowerShell script
│   │   ├── test.sh                         # Test runner script
│   │   └── test-api.sh                     # API integration tests
│   ├── PopsicleFactory.postman_collection.json  # Postman collection
│   ├── .gitignore                          # Git ignore rules
├── PopsicleFactory.sln                     # Solution file
└── README.md                               # This file
```

## API Documentation

### Endpoints Overview

| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| GET | `/api/popsicle` | Get all popsicles | None | `PopsicleViewModel[]` |
| GET | `/api/popsicle/{id}` | Get popsicle by ID | None | `PopsicleViewModel` |
| POST | `/api/popsicle` | Create new popsicle | `CreatePopsicleDto` | `PopsicleViewModel` |
| PUT | `/api/popsicle/{id}` | Replace popsicle | `CreatePopsicleDto` | `PopsicleViewModel` |
| PATCH | `/api/popsicle/{id}` | Partially update | `UpdatePopsicleDto` | `PopsicleViewModel` |
| DELETE | `/api/popsicle/{id}` | Delete popsicle | None | `204 No Content` |
| GET | `/api/popsicle/search` | Search popsicles | Query parameters | `PopsicleViewModel[]` |

### Data Models

#### Popsicle (Core Model)
```csharp
{
  "id": 1,
  "name": "Classic Vanilla",
  "flavor": "Vanilla", 
  "price": 2.99,
  "description": "Creamy vanilla popsicle",
  "quantity": 50,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

#### CreatePopsicleDto
```csharp
{
  "name": "string",        // Required, max 100 chars
  "flavor": "string",      // Required, max 50 chars
  "price": 0.01,          // Required, 0.01 - 999.99
  "description": "string", // Optional, max 500 chars
  "quantity": 0           // Required, non-negative
}
```

#### UpdatePopsicleDto (Partial Update)
```csharp
{
  "name": "string",        // Optional
  "flavor": "string",      // Optional
  "price": 0.01,          // Optional
  "description": "string", // Optional
  "quantity": 0           // Optional
}
```

### Search Parameters
- `name`: Partial name match (case-insensitive)
- `flavor`: Partial flavor match (case-insensitive)
- `minPrice`/`maxPrice`: Price range filtering
- `minQuantity`/`maxQuantity`: Quantity range filtering

### Error Responses

#### Validation Error (400)
```json
{
  "message": "The popsicle request is invalid",
  "errors": [
    "Name is required",
    "Price must be between 0.01 and 999.99"
  ],
  "timestamp": "2024-01-01T00:00:00Z"
}
```

#### Not Found (404)
```json
{
  "message": "Popsicle with ID 999 does not exist",
  "timestamp": "2024-01-01T00:00:00Z"
}
```

## Development Guide

### Running the Application

#### Local Development
```bash
# Standard run
./run.sh

# Development with hot reload
./run.sh dev

# Manual run
dotnet run --project API
```

### Configuration

#### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: Override default ports

### Adding New Features

1. **Add Model**: Create new model in `Models/`
2. **Create DTOs**: Add request/response DTOs in `Models/DTOs/`
3. **Define Interface**: Create service interface in `Services/Interfaces/`
4. **Implement Service**: Add business logic in `Services/`
5. **Create Repository**: Add data access in `Repositories/`
6. **Build Controller**: Create API endpoints in `Controllers/`
7. **Write Tests**: Add comprehensive tests in `API.Tests/`
8. **Update Documentation**: Modify README and XML comments

## Testing

### Unit Testing
```bash
# Run all tests
./test.sh

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "PopsicleControllerTests"
```

### Integration Testing
```bash
# HTTP requests (Visual Studio/VS Code)
# Open http-requests/popsicle_api.http

# cURL commands
./scripts/test-api.sh

# Postman collection
# Import PopsicleFactory.postman_collection.json
```

### Test Coverage
After running `./test.sh`, coverage reports are generated in:
- `TestResults/CoverageReport/index.html`

### Sample Data
The application includes pre-loaded sample popsicles:
- Classic Vanilla ($2.99, 50 in stock)
- Strawberry Delight ($3.49, 30 in stock)
- Chocolate Fudge ($3.99, 25 in stock)
- Orange Burst ($2.79, 40 in stock)

## Deployment

### Local Deployment
```bash
# Build for production
dotnet build --configuration Release

# Run in production mode
ASPNETCORE_ENVIRONMENT=Production dotnet run --project API
```

### Production Considerations
- Replace in-memory storage with persistent database
- Add authentication and authorization
- Implement rate limiting
- Configure proper logging
- Set up health checks and monitoring
- Use HTTPS certificates

## Scripts Reference

### Setup and Build
```bash
./setup.sh              # Initial project setup
dotnet restore           # Restore NuGet packages
dotnet build            # Build solution
```

### Development
```bash
./run.sh                # Start API server
./run.sh dev           # Start with hot reload
dotnet watch run       # Manual hot reload
```

### Testing
```bash
./test.sh              # Run tests with coverage
dotnet test            # Run tests only
./scripts/test-api.sh  # Integration tests via cURL
```
## Technologies Used

- **Framework**: ASP.NET Core 8
- **Language**: C# 12
- **Architecture**: Clean Architecture, SOLID principles
- **Testing**: xUnit, Moq
- **Documentation**: Swagger/OpenAPI 3.0
- **Data Storage**: In-memory (ConcurrentDictionary)

## Design Decisions

### In-Memory Storage
Using `ConcurrentDictionary` for thread-safe operations. Production apps should use:
- SQL Server with Entity Framework Core
- PostgreSQL with Npgsql
- MongoDB with official driver

### Repository Pattern
Abstracts data access for easy testing and future database integration.

### Service Layer
Separates business logic from HTTP concerns and data access.

### DTO Pattern
Provides stable API contracts separate from internal models.

### Async/Await
All operations are async for better scalability.

### Global Exception Handling
Centralized error handling with consistent response format.

## Future Enhancements

### Phase 1 - Database Integration
- Entity Framework Core setup
- SQL Server/PostgreSQL support  
- Database migrations
- Audit logging

### Phase 2 - Advanced Features
- Authentication & Authorization (JWT)
- Role-based access control
- Advanced search with filtering
- Pagination support
- Bulk operations

### Phase 3 - Production Features
- Caching layer (Redis)
- Rate limiting
- API versioning
- Health checks and monitoring
- Distributed logging

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Follow the existing code style and patterns
4. Write comprehensive tests
5. Update documentation
6. Submit a pull request

### Code Standards
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Write XML documentation for public APIs
- Maintain test coverage above 80%
- Follow SOLID principles

### Commit Messages
- Use conventional commits format
- Start with type: feat, fix, docs, test, refactor
- Be descriptive but concise

## Troubleshooting

### Common Issues

#### Port Already in Use
```bash
# Find process using port 7212
netstat -ano | findstr :7212  # Windows
lsof -i :7212                 # macOS/Linux

# Kill process and restart
```

#### SSL Certificate Issues
```bash
# Trust development certificates
dotnet dev-certs https --trust
```

#### Package Restore Issues
```bash
# Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore
```
