# Firebase Authentication Setup Guide

This guide explains how to set up Firebase Authentication in your Clean Architecture .NET project.

## Prerequisites

1. A Firebase project (create one at https://console.firebase.google.com/)
2. Firebase Admin SDK service account key
3. .NET 8.0 SDK

## Setup Steps

### 1. Firebase Project Setup

1. Go to [Firebase Console](https://console.firebase.google.com/)
2. Create a new project or select an existing one
3. Enable Authentication in the Firebase console
4. Go to Project Settings > Service Accounts
5. Click "Generate new private key" to download the service account JSON file
6. Save this file as `firebase-service-account-key.json` in your project root

### 2. Configuration

Update the `appsettings.json` file with your Firebase project details:

```json
{
  "Firebase": {
    "ServiceAccountKeyPath": "firebase-service-account-key.json",
    "ProjectId": "your-firebase-project-id"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-here-minimum-16-characters-long",
    "Issuer": "CleanArchitecture",
    "Audience": "CleanArchitecture",
    "ExpiryInHours": 1
  }
}
```

### 3. Firebase Service Account Key

Place your `firebase-service-account-key.json` file in the project root. The file should look like this:

```json
{
  "type": "service_account",
  "project_id": "your-project-id",
  "private_key_id": "your-private-key-id",
  "private_key": "-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----\n",
  "client_email": "firebase-adminsdk-xxxxx@your-project-id.iam.gserviceaccount.com",
  "client_id": "your-client-id",
  "auth_uri": "https://accounts.google.com/o/oauth2/auth",
  "token_uri": "https://oauth2.googleapis.com/token",
  "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
  "client_x509_cert_url": "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-xxxxx%40your-project-id.iam.gserviceaccount.com"
}
```

## API Endpoints

### Authentication Endpoints

#### 1. Firebase Login
```
POST /api/auth/firebase-login
Content-Type: application/json

{
  "idToken": "firebase-id-token-from-client"
}
```

#### 2. User Registration
```
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123",
  "name": "John Doe"
}
```

#### 3. Token Refresh
```
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "refresh-token-from-previous-login"
}
```

#### 4. Get Current User
```
GET /api/auth/me
Authorization: Bearer your-jwt-token
```

#### 5. Logout
```
POST /api/auth/logout
Authorization: Bearer your-jwt-token
```

## Client-Side Integration

### JavaScript/TypeScript Example

```javascript
// Initialize Firebase (add your config)
import { initializeApp } from 'firebase/app';
import { getAuth, signInWithEmailAndPassword, createUserWithEmailAndPassword } from 'firebase/auth';

const firebaseConfig = {
  apiKey: "your-api-key",
  authDomain: "your-project.firebaseapp.com",
  projectId: "your-project-id",
  storageBucket: "your-project.appspot.com",
  messagingSenderId: "123456789",
  appId: "your-app-id"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

// Login function
async function loginWithFirebase(email, password) {
  try {
    const userCredential = await signInWithEmailAndPassword(auth, email, password);
    const idToken = await userCredential.user.getIdToken();
    
    // Send token to your API
    const response = await fetch('/api/auth/firebase-login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ idToken })
    });
    
    const authResponse = await response.json();
    // Store JWT token for API calls
    localStorage.setItem('accessToken', authResponse.accessToken);
    localStorage.setItem('refreshToken', authResponse.refreshToken);
    
    return authResponse;
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
}

// Register function
async function registerWithFirebase(email, password, name) {
  try {
    const userCredential = await createUserWithEmailAndPassword(auth, email, password);
    const idToken = await userCredential.user.getIdToken();
    
    // Send registration data to your API
    const response = await fetch('/api/auth/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password, name })
    });
    
    const authResponse = await response.json();
    localStorage.setItem('accessToken', authResponse.accessToken);
    localStorage.setItem('refreshToken', authResponse.refreshToken);
    
    return authResponse;
  } catch (error) {
    console.error('Registration error:', error);
    throw error;
  }
}
```

## Security Considerations

1. **JWT Secret Key**: Use a strong, randomly generated secret key for JWT signing
2. **Firebase Service Account**: Keep your service account key secure and never commit it to version control
3. **Token Expiration**: Set appropriate token expiration times
4. **HTTPS**: Always use HTTPS in production
5. **CORS**: Configure CORS properly for your frontend domain

## Environment Variables (Recommended)

For production, use environment variables instead of hardcoded values:

```bash
# JWT Configuration
JWT_SECRET_KEY=your-super-secret-key-here-minimum-16-characters-long
JWT_ISSUER=CleanArchitecture
JWT_AUDIENCE=CleanArchitecture

# Firebase Configuration
FIREBASE_PROJECT_ID=your-firebase-project-id
FIREBASE_SERVICE_ACCOUNT_KEY_PATH=path/to/service-account-key.json
```

## Troubleshooting

### Common Issues

1. **Firebase initialization error**: Ensure your service account key is valid and accessible
2. **JWT token validation error**: Check that your JWT secret key is at least 16 characters long
3. **CORS errors**: Configure CORS in your API to allow requests from your frontend
4. **Token expiration**: Implement proper token refresh logic on the client side

### Debug Mode

Enable detailed logging by setting the log level to Debug in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "CleanArchitecture.Infrastructure.Services": "Debug"
    }
  }
}
```

## Testing

You can test the authentication endpoints using tools like Postman or curl:

```bash
# Test Firebase login
curl -X POST http://localhost:5000/api/auth/firebase-login \
  -H "Content-Type: application/json" \
  -d '{"idToken": "your-firebase-id-token"}'

# Test registration
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email": "test@example.com", "password": "password123", "name": "Test User"}'
``` 