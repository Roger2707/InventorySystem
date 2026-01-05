# Inventory System - Clean Architecture

A modern ASP.NET Core application built with Clean Architecture principles.

## Project Structure

This solution follows Clean Architecture with the following layers:

### 📁 InventorySystem.Domain
**Core business logic layer** - No dependencies on other projects
- `Entities/` - Domain entities (e.g., `BaseEntity`)
- `Common/` - Shared domain objects (e.g., `Result`)

### 📁 InventorySystem.Application
**Application business logic layer** - Depends only on Domain
- `Interfaces/` - Repository and service interfaces
- `DTOs/` - Data Transfer Objects (to be added)
- `UseCases/` - Application use cases (to be added)
- `Mappings/` - AutoMapper profiles (to be added)

### 📁 InventorySystem.Infrastructure
**Infrastructure layer** - Implements Application interfaces
- `Data/` - DbContext and database configuration
- `Repositories/` - Repository implementations
- `Services/` - External service implementations (to be added)

### 📁 InventorySystem.WebApi
**Presentation layer** - API controllers and configuration
- `Controllers/` - API controllers
- `Program.cs` - Application startup and DI configuration

## Architecture Principles

1. **Dependency Rule**: Dependencies point inward
   - Domain has no dependencies
   - Application depends on Domain
   - Infrastructure depends on Application
   - WebApi depends on Application and Infrastructure

2. **Separation of Concerns**: Each layer has a specific responsibility

3. **Dependency Injection**: Used throughout for loose coupling

## Technology Stack

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 9.0** - ORM for data access
- **SQLite** - Database (can be easily changed to SQL Server, PostgreSQL, etc.)
- **Swagger/OpenAPI** - API documentation

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022, VS Code, or Rider

### Running the Application

1. Restore packages:
   ```bash
   dotnet restore
   ```

2. Build the solution:
   ```bash
   dotnet build
   ```

3. Run the Web API:
   ```bash
   cd InventorySystem.WebApi
   dotnet run
   ```

4. Access Swagger UI:
   - Navigate to `https://localhost:5001/swagger` (or the port shown in console)

### Database

The application uses SQLite by default. The database file (`inventory.db`) will be created automatically on first run.

To change the database provider, update the connection string in `appsettings.json` and modify `Program.cs` to use the appropriate EF Core provider.

## Project Dependencies

```
InventorySystem.WebApi
  ├── InventorySystem.Application
  │     └── InventorySystem.Domain
  └── InventorySystem.Infrastructure
        └── InventorySystem.Application
              └── InventorySystem.Domain
```

## Features

- ✅ Clean Architecture structure
- ✅ Generic Repository pattern
- ✅ Unit of Work pattern
- ✅ Soft delete support
- ✅ Automatic timestamp tracking (CreatedAt, UpdatedAt)
- ✅ Dependency Injection configured
- ✅ Swagger/OpenAPI documentation
- ✅ CORS enabled

## Next Steps

To extend this project:

1. **Add Domain Entities**: Create entities in `InventorySystem.Domain/Entities/`
2. **Create DTOs**: Add DTOs in `InventorySystem.Application/DTOs/`
3. **Implement Use Cases**: Add use cases in `InventorySystem.Application/UseCases/`
4. **Configure Entities**: Add EF Core configurations in `InventorySystem.Infrastructure/Data/Configurations/`
5. **Create Controllers**: Add API controllers in `InventorySystem.WebApi/Controllers/`

## License

This project is open source and available for use.

