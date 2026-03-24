# Inventory System Backend (Clean Architecture)

Backend e-commerce/inventory trên ASP.NET Core Web API theo Clean Architecture. Hệ thống dùng **EF Core cho write model (CRUD/transaction)** và **Dapper cho read model (query/report phức tạp)**.

## Cấu trúc sau khi refactor

```text
InventorySystem/
├─ src/
│  ├─ InventorySystem.Domain/
│  ├─ InventorySystem.Application/
│  ├─ InventorySystem.Infrastructure/
│  └─ InventorySystem.WebApi/
├─ InventorySystem.sln
└─ README.md
```

## Vai trò từng layer

### `src/InventorySystem.Domain`
- Core business: entities, value objects, rules nghiệp vụ.
- Không phụ thuộc project nào khác.

### `src/InventorySystem.Application`
- Use cases, service contracts, DTOs, orchestration.
- Chỉ phụ thuộc `Domain`.

### `src/InventorySystem.Infrastructure`
- Triển khai persistence và external concerns.
- Chứa EF Core repositories, UnitOfWork, Dapper queries, JWT/Redis services.
- Phụ thuộc `Application` và `Domain`.

### `src/InventorySystem.WebApi`
- Presentation + composition root (DI, middleware, controllers).
- Phụ thuộc `Application` và `Infrastructure`.

## Dependency rule

```text
WebApi -> Application
WebApi -> Infrastructure
Infrastructure -> Application -> Domain
```

## Data access strategy

- `EF Core`: create/update/delete, transaction boundaries, migrations.
- `Dapper`: query tối ưu cho dashboard/report/list phức tạp.
- Khi cần nhất quán dữ liệu trong transaction, Dapper dùng cùng connection/transaction với EF.

## Chạy dự án

```bash
dotnet restore
dotnet build InventorySystem.sln
dotnet run --project src/InventorySystem.WebApi
```

Swagger UI: `/swagger`


