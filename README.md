# Inventory System (Clean Architecture)

Solution ASP.NET Core Web API theo Clean Architecture, dùng **EF Core cho CRUD** và thêm **Dapper cho các truy vấn SELECT/JOIN phức tạp**.

## Project structure (thực tế trong repo)

### `InventorySystem.Domain`
- **Vai trò**: core domain, entity, business rules (không phụ thuộc project khác)
- **Folder chính**:
  - `Entities/`
  - `Common/` (vd: `Result`)

### `InventorySystem.Application`
- **Vai trò**: application layer (use-case/services), chỉ phụ thuộc `Domain`
- **Quy ước folder**:
  - `DTOs/`: DTOs trả về/nhận vào từ WebApi
  - `Interfaces/`: toàn bộ contracts mà `Infrastructure` sẽ implement
    - `Interfaces/Persistence/` (Repository, UnitOfWork) *(hiện đang để chung trong `Interfaces/`)*  
    - `Interfaces/Services/`: **service contracts** (`IUserService`, `IRoleService`, `IWarehouseService`, ...)
    - `Interfaces/Queries/`: **read-only query contracts** (phục vụ Dapper)
  - `Services/`: **service implementations** (không chứa interface nữa)

### `InventorySystem.Infrastructure`
- **Vai trò**: persistence + implementation các interface từ `Application`
- **Folder chính**:
  - `Data/`: `ApplicationDbContext`
  - `Repositories/`: EF Core repositories + `UnitOfWork`
  - `Services/`: cross-cutting infra services (vd: JWT, PasswordHasher)
  - `Queries/`: **Dapper query implementations** (read-model)
  - `Migrations/`, `Seed/`

### `InventorySystem.WebApi`
- **Vai trò**: Presentation + Composition Root (DI, middleware, controllers)
- **Folder chính**:
  - `Controllers/`
  - `Middleware/`
  - `Policies/`
  - `Program.cs`

## Dependency rule (Clean Architecture)

```
WebApi -> Application
WebApi -> Infrastructure
Infrastructure -> Application -> Domain
```

## Data access: EF Core + Dapper

- **EF Core dùng cho**:
  - CRUD (Create/Update/Delete)
  - Transaction + UnitOfWork
  - Soft-delete filter + audit timestamps (CreatedAt/UpdatedAt)

- **Dapper dùng cho**:
  - SELECT phức tạp (join nhiều bảng, projection DTO, paging/report)
  - Tối ưu query tránh N+1 (vd: lấy Users kèm Roles bằng 1 query)

### Có cần “share DbContext” giữa EF Core và Dapper không?

- **Không cần share `DbContext` như một ORM context** (Dapper không dùng change tracking).
- **Nên share cùng `DbConnection`/`DbTransaction` khi cần tính nhất quán**:
  - **Có share** (khuyến nghị trong scope transaction): khi bạn đang `BeginTransactionAsync()` ở EF/UnitOfWork và muốn Dapper query nằm **cùng transaction** → lấy connection/transaction từ `ApplicationDbContext` (đảm bảo cùng connection + cùng transaction).
  - **Không share** (ok cho read-only độc lập): query chỉ đọc, không cần nhất quán với các thay đổi đang pending trong EF (hoặc chấp nhận read-committed bình thường) → Dapper có thể mở connection từ connection string riêng.

Trong repo này, Dapper query service đang lấy connection/transaction từ EF Core:
- `InventorySystem.Infrastructure/Queries/UserQueries.cs`
- `InventorySystem.Infrastructure/Queries/DapperExecutor.cs` (helper dùng chung)

### Dùng “1 hàm get data luôn” với DapperExecutor

Bạn có thể inject `IDapperExecutor` và gọi trực tiếp:

- `QueryAsync<T>`: SELECT list
- `QuerySingleOrDefaultAsync<T>`: SELECT 1 dòng
- `ExecuteAsync`: INSERT/UPDATE/DELETE (nếu muốn)

## Cấu hình database

- Connection string: `InventorySystem.WebApi/appsettings.json` (`ConnectionStrings:DefaultConnection`)
- Provider: **SQL Server** (EF Core `UseSqlServer`)
- App startup hiện đang tự chạy:
  - `db.Database.Migrate()`
  - seed data

## Chạy project

```bash
dotnet restore
dotnet build
dotnet run --project InventorySystem.WebApi
```

Swagger UI: `/swagger`

## Ghi chú bảo mật

- `appsettings.json` hiện có `JWTSettings:TokenKey`. Nếu deploy thật, nên chuyển sang **User Secrets / Environment Variables** và không commit key thật.


