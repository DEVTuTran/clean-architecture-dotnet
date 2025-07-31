# Clean Architecture .NET - Kaburaya System

Dự án Clean Architecture .NET được cấu trúc lại dựa trên database schema Kaburaya, một hệ thống quản lý tổ chức và khách hàng.

## Cấu trúc dự án

### Domain Layer
- **Entities**: Các entity chính của hệ thống
  - `Organization`: Tổ chức
  - `User`: Người dùng
  - `Client`: Khách hàng
  - `ClientData`: Dữ liệu khách hàng
  - `PaymentCollection`: Thanh toán
  - `AuditLog`: Nhật ký hoạt động
  - Và các entity phụ trợ khác

- **Repositories**: Interface định nghĩa các thao tác CRUD
  - `IOrganizationRepository`
  - `IUserRepository`
  - `IClientRepository`

### Application Layer
- **Commands**: Các command để thực hiện thao tác
- **Queries**: Các query để lấy dữ liệu
- **Handlers**: Xử lý các command và query
- **DTOs**: Data Transfer Objects
- **Mappings**: AutoMapper profiles

### Infrastructure Layer
- **Data**: Entity Framework DbContext
- **Repositories**: Implementation của các repository interface

### API Layer
- **Controllers**: REST API endpoints

## Cài đặt và chạy

### Yêu cầu
- .NET 8.0
- MySQL Server
- Entity Framework Core Tools

### Cài đặt database
1. Tạo database MySQL với tên `kaburaya`
2. Chạy script SQL từ file `kaburaya.sql`
3. Cập nhật connection string trong `appsettings.json`

### Chạy ứng dụng
```bash
cd src/CleanArchitecture.API
dotnet run
```

## API Endpoints

### Organizations
- `GET /api/organizations` - Lấy danh sách tổ chức
- `GET /api/organizations/{id}` - Lấy tổ chức theo ID
- `POST /api/organizations` - Tạo tổ chức mới
- `PUT /api/organizations/{id}` - Cập nhật tổ chức
- `DELETE /api/organizations/{id}` - Xóa tổ chức

## Cấu trúc Database

Hệ thống sử dụng MySQL với các bảng chính:
- `organization`: Thông tin tổ chức
- `user`: Người dùng hệ thống
- `client`: Khách hàng
- `client_data`: Dữ liệu khách hàng
- `payment_collection`: Lịch sử thanh toán
- `audit_log`: Nhật ký hoạt động

## Clean Architecture Principles

Dự án tuân thủ các nguyên tắc Clean Architecture:
- **Dependency Inversion**: Domain layer không phụ thuộc vào Infrastructure
- **Separation of Concerns**: Mỗi layer có trách nhiệm riêng biệt
- **Testability**: Dễ dàng test từng layer độc lập
- **Maintainability**: Code dễ bảo trì và mở rộng
