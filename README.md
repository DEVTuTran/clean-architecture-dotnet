# Clean Architecture .NET Solution

This is a comprehensive .NET solution implementing Clean Architecture principles with CQRS pattern using MediatR.

## Architecture Overview

The solution follows Clean Architecture principles with the following layers:

### 🏗️ Project Structure

```
CleanArchitecture/
├── src/
│   ├── CleanArchitecture.Domain/           # Core business logic and entities
│   ├── CleanArchitecture.Application/      # Use cases and business rules
│   ├── CleanArchitecture.Infrastructure/   # External concerns (DB, APIs)
│   └── CleanArchitecture.API/              # Presentation layer (Web API)
├── tests/
│   ├── CleanArchitecture.Domain.Tests/     # Domain unit tests
│   ├── CleanArchitecture.Application.Tests/ # Application unit tests
│   └── CleanArchitecture.Integration.Tests/ # Integration tests
└── CleanArchitecture.sln                   # Solution file
```

### 📁 Layer Details

#### Domain Layer (`CleanArchitecture.Domain`)

- **Entities**: Core business objects (Product, BaseEntity)
- **Repositories**: Abstract interfaces for data access
- **Exceptions**: Domain-specific exceptions
- **No dependencies** on other layers

#### Application Layer (`CleanArchitecture.Application`)

- **DTOs**: Data Transfer Objects for API communication
- **Interfaces**: Application service contracts
- **Commands/Queries**: CQRS implementation with MediatR
- **Handlers**: Command and Query handlers
- **Services**: Application business logic
- **Mappings**: AutoMapper profiles
- **Depends only** on Domain layer

#### Infrastructure Layer (`CleanArchitecture.Infrastructure`)

- **Data**: Entity Framework DbContext and configurations
- **Repositories**: Concrete implementations of repository interfaces
- **DependencyInjection**: Service registration
- **Depends on** Application and Domain layers

#### API Layer (`CleanArchitecture.API`)

- **Controllers**: REST API endpoints
- **Program.cs**: Application startup and configuration
- **Depends on** Application and Infrastructure layers

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd CleanArchitecture
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the solution**

   ```bash
   dotnet build
   ```

4. **Run the API**

   ```bash
   cd src/CleanArchitecture.API
   dotnet run
   ```

5. **Access Swagger UI**
   - Navigate to `https://localhost:7001/swagger`
   - Or `http://localhost:5001/swagger`

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/CleanArchitecture.Domain.Tests/
dotnet test tests/CleanArchitecture.Application.Tests/
dotnet test tests/CleanArchitecture.Integration.Tests/
```

## 🛠️ Key Technologies

- **.NET 8.0**: Latest .NET framework
- **Entity Framework Core**: ORM for data access
- **MediatR**: Implementation of CQRS pattern
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for tests

## 📋 API Endpoints

The API provides the following endpoints for Product management:

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update existing product
- `DELETE /api/products/{id}` - Delete product

### Sample API Usage

#### Create a Product

```bash
curl -X POST "https://localhost:7001/api/products" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Sample Product",
       "description": "A sample product description",
       "price": 99.99,
       "stockQuantity": 10,
       "category": "Electronics"
     }'
```

#### Get All Products

```bash
curl -X GET "https://localhost:7001/api/products"
```

## 🏛️ Clean Architecture Principles

This solution implements the following Clean Architecture principles:

1. **Dependency Rule**: Dependencies point inward. Domain has no dependencies, API depends on all layers.

2. **Separation of Concerns**: Each layer has a specific responsibility:

   - Domain: Business entities and rules
   - Application: Use cases and orchestration
   - Infrastructure: External concerns
   - API: Presentation and HTTP concerns

3. **CQRS Pattern**: Commands and Queries are separated using MediatR:

   - Commands: Modify state (Create, Update, Delete)
   - Queries: Read data (Get, List)

4. **Repository Pattern**: Abstract data access through interfaces

5. **Dependency Injection**: Loose coupling through DI container

## 🧪 Testing Strategy

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test component interactions
- **Test Coverage**: Focus on business logic and critical paths

## 🔧 Configuration

The application uses:

- **In-Memory Database**: For development and testing
- **SQL Server**: Can be configured for production
- **Environment-based settings**: Different configurations for different environments

## 📈 Future Enhancements

- Add authentication and authorization
- Implement caching with Redis
- Add logging with Serilog
- Implement event sourcing
- Add health checks
- Configure CI/CD pipeline

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## 📄 License

This project is licensed under the MIT License.
