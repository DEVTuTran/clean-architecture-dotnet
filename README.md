# Clean Architecture .NET Project

A .NET 8 Clean Architecture project with Firebase Authentication support.

## Features

- **Clean Architecture** with Domain, Application, Infrastructure, and API layers
- **Firebase Authentication** for user registration and login
- **JWT Token** authentication for API access
- **Entity Framework Core** with MySQL
- **MediatR** for CQRS pattern
- **AutoMapper** for object mapping
- **Swagger/OpenAPI** documentation

## Authentication Endpoints

### Firebase Authentication

- `POST /api/auth/firebase-login` - Login with Firebase ID token
- `POST /api/auth/register` - Register new user with Firebase
- `POST /api/auth/refresh-token` - Refresh JWT token
- `GET /api/auth/me` - Get current user information
- `POST /api/auth/logout` - Logout (client-side)

### Test Endpoints

- `GET /api/test/public` - Public endpoint (no auth required)
- `GET /api/test/authenticated` - Protected endpoint (auth required)
- `GET /api/test/optional` - Optional authentication endpoint

## Setup

1. **Firebase Configuration**

   - Create a Firebase project at [Firebase Console](https://console.firebase.google.com/)
   - Download your service account key and save as `firebase-service-account-key.json`
   - Update `appsettings.json` with your Firebase project details

2. **Database**

   - Update the connection string in `appsettings.json`
   - Run migrations: `dotnet ef database update`

3. **JWT Configuration**
   - Update the JWT secret key in `appsettings.json`
   - Ensure the secret key is at least 16 characters long

## Running the Application

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the API
cd src/CleanArchitecture.API
dotnet run
```

The API will be available at `https://localhost:7001` with Swagger documentation at `/swagger`.

## Firebase Setup

See [FIREBASE_SETUP.md](FIREBASE_SETUP.md) for detailed Firebase configuration instructions.

## Project Structure

```
src/
├── CleanArchitecture.API/          # Web API layer
├── CleanArchitecture.Application/  # Application layer (CQRS, DTOs, Mappings)
├── CleanArchitecture.Domain/       # Domain layer (Entities, Repositories)
└── CleanArchitecture.Infrastructure/ # Infrastructure layer (DbContext, Services)
```

## Authentication Flow

1. **Client** authenticates with Firebase (email/password, Google, etc.)
2. **Client** sends Firebase ID token to `/api/auth/firebase-login`
3. **API** verifies Firebase token and creates/updates user in database
4. **API** returns JWT token for subsequent API calls
5. **Client** uses JWT token in `Authorization: Bearer` header

## Security

- Firebase handles user authentication securely
- JWT tokens for API authorization
- HTTPS required in production
- Service account keys should never be committed to version control
