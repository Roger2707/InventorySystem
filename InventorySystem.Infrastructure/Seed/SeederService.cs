using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Entities.Accounts;
using InventorySystem.Domain.Entities.GoodsReceipt;
using InventorySystem.Domain.Entities.Identity;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Domain.Entities.Products;
using InventorySystem.Domain.Entities.PurchaseOrder;
using InventorySystem.Domain.Entities.Suppliers;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Domain.Entities.Delivery;
using InventorySystem.Domain.Entities.SalesOrder;
using InventorySystem.Application.DTOs.Invoices;
using InventorySystem.Application.Extensions;

namespace InventorySystem.Infrastructure.Seed
{
    public class SeederService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDeliveryService _deliveryService;
        private readonly IInvoiceService _invoiceService;
        private readonly IPasswordHasher _passwordHasher;

        public SeederService(ApplicationDbContext context, IDeliveryService deliveryService, IInvoiceService invoiceService, IPasswordHasher passwordHasher)
        {
            _context = context;
            _deliveryService = deliveryService;
            _invoiceService = invoiceService;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedDataAsync()
        {

            if (!await _context.Accounts.AnyAsync())
                await SeedAccoutsAsync();

            if (!await _context.Users.AnyAsync())
                await SeedUserAsyc();

            if (!await _context.Roles.AnyAsync())
                await SeedRoleAsync();

            if (await _context.Users.AnyAsync() && await _context.Roles.AnyAsync() && !await _context.UserRoles.AnyAsync())
                await SeedUserRoleAsync();

            if (!await _context.Regions.AnyAsync())
                await SeedRegionAsync();

            if (await _context.Users.AnyAsync() && await _context.Regions.AnyAsync() && !await _context.UserRegions.AnyAsync())
                await SeedUserRegionsAsync();

            if (!await _context.Warehouses.AnyAsync())
                await SeedWarehouseAsync();

            if (await _context.Users.AnyAsync() && await _context.Warehouses.AnyAsync() && !await _context.UserWarehouses.AnyAsync())
                await SeedUserWarehousesAsync();

            if (!await _context.Permissions.AnyAsync())
                await SeedPermissionsAsync();

            if (await _context.Roles.AnyAsync() && await _context.Permissions.AnyAsync() && !await _context.RolePermissions.AnyAsync())
                await SeedRolePermissionsAsync();

            if (!await _context.Customers.AnyAsync())
                await SeedCustomersAsync();

            if (!await _context.Suppliers.AnyAsync())
                await SeedSuppliersAsync();

            if (!await _context.Categories.AnyAsync())
                await SeedCategoriesAsync();

            if (!await _context.UoMs.AnyAsync())
                await SeedUoMsAsync();

            if (!await _context.Products.AnyAsync())
                await SeedProductsAsync();

            if (await _context.Products.AnyAsync() && await _context.UoMs.AnyAsync() && !await _context.ProductUoMConversions.AnyAsync())
                await SeedProductConversionsAsync();

            if (!await _context.SupplierProductPrices.AnyAsync())
                await SeedSupplierProductPricesAsync();

            if (!await _context.PurchaseOrders.AnyAsync())
                await SeedPurchaseOrdersAsync();

            if (!await _context.GoodsReceipts.AnyAsync())
                await SeedGoodsReceiptsAsync();

            if (!await _context.SalesOrders.AnyAsync())
                await SeedSalesOrdersAsync();

            if (!await _context.Deliveries.AnyAsync())
                await SeedDeliveriesAsync();

            if (!await _context.Invoices.AnyAsync())
                await SeedInvoicesAsync();
        }

        private async Task SeedAccoutsAsync()
        {
            var accounts = new List<Account>
            {
                new Account
                {
                    Code = "1000",
                    Name = "Cash",
                    Type = AccountType.Asset,
                    IsActive = true
                },
                new Account
                {
                    Code = "1100",
                    Name = "Accounts Receivable",
                    Type = AccountType.Asset,
                    IsActive = true
                },
                new Account
                {
                    Code = "1200",
                    Name = "Inventory",
                    Type = AccountType.Asset,
                    IsActive = true
                },
                new Account
                {
                    Code = "4000",
                    Name = "Revenue",
                    Type = AccountType.Revenue,
                    IsActive = true
                },
                new Account
                {
                    Code = "5000",
                    Name = "Cost Of Goods Sold",
                    Type = AccountType.Expense,
                    IsActive = true
                }
            };

            foreach (var acc in accounts)
                _context.Accounts.Add(acc);

            await _context.SaveChangesAsync();
        }

        private async Task SeedUserAsyc()
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "rogersa",
                    FullName = "Roger SA",
                    Email = "rogersa@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "01 Le Duan, P.Ben Thanh, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Rogersa@123"),
                },
                new User
                {
                    Username = "greatorm",
                    FullName = "Greato RM",
                    Email = "greatorm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "03 PDP, P.Cau Kieu, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Greatorm@123"),
                },
                new User
                {
                    Username = "baoanrm",
                    FullName = "Bao An RM",
                    Email = "baoanrm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "05 Van Kiep, P.Phu Nhuan, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Baoanrm@123"),
                },
                new User
                {
                    Username = "gapuwm",
                    FullName = "Gapu Truong WM",
                    Email = "gapuwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "07 Tan Dinh, P.Tan Dinh, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Gapuwm@123"),
                },
                new User
                {
                    Username = "nguyenwm",
                    FullName = "Truong Nguyen WM",
                    Email = "nguyenwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "09 Phan Dang Luu, P.Thanh My Tay, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Nguyenwm@123"),
                },
                new User
                {
                    Username = "duthwm",
                    FullName = "Duc Thang WM",
                    Email = "duthwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "11 Quang Trung, P.Go Vap, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Duthwm@123"),
                },
                new User
                {
                    Username = "quincyst",
                    FullName = "Quincy ST",
                    Email = "quincyst@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "13 Pham Van Dong, P.Thu Duc, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Quincyst@123"),
                },
                new User
                {
                    Username = "alliest",
                    FullName = "Thuy Hang ST",
                    Email = "alliest@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "15 Ben Van Don, P.Pham Ngu Lao, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Alliest@123"),
                },
                new User
                {
                    Username = "mist",
                    FullName = "Nhu Quynh ST",
                    Email = "mist@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "17 Hoang Hoa Tham, P.Bien Hoa, HCMC",
                    PasswordHash = _passwordHasher.HashPassword("Mist@123"),
                },
            };

            foreach (var user in users)
                _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        private async Task SeedRoleAsync()
        {
            var roles = new List<Role>
            {
                new Role
                {
                    RoleName = "Super_Admin",
                    Description = "Full Access anywhere in application.",
                    RoleLevel = RoleLevel.SuperAdmin,
                },
                new Role
                {
                    RoleName = "Regional_Manager",
                    Description = "Full Access any warehouse in area they manage.",
                    RoleLevel = RoleLevel.RegionalManager,
                },
                new Role
                {
                    RoleName = "Warehouse_Manager",
                    Description = "Full Access in warehouse they manage.",
                    RoleLevel = RoleLevel.WarehouseManager,
                },
                new Role
                {
                    RoleName = "Staff",
                    Description = "Limit access in warehouse they work.",
                    RoleLevel = RoleLevel.Staff,
                },
            };

            foreach (var role in roles)
                _context.Roles.Add(role);

            await _context.SaveChangesAsync();
        }

        private async Task SeedUserRoleAsync()
        {
            var userRoles = new List<UserRole>
            {
                new UserRole
                {
                    UserId = 1,
                    RoleId = 1,
                },
                new UserRole
                {
                    UserId = 2,
                    RoleId = 2,
                },
                new UserRole
                {
                    UserId = 3,
                    RoleId = 2,
                },
                new UserRole
                {
                    UserId = 4,
                    RoleId = 3,
                },
                new UserRole
                {
                    UserId = 5,
                    RoleId = 3,
                },
                new UserRole
                {
                    UserId = 6,
                    RoleId = 3,
                },
                new UserRole
                {
                    UserId = 7,
                    RoleId = 4,
                },
                new UserRole
                {
                    UserId = 8,
                    RoleId = 4,
                },
                new UserRole
                {
                    UserId = 9,
                    RoleId = 4,
                },
            };

            foreach (var ur in userRoles)
                _context.UserRoles.Add(ur);

            await _context.SaveChangesAsync();
        }

        private async Task SeedRegionAsync()
        {
            var regions = new List<Region>
            {
                new Region {RegionCode = "RE - 001", RegionName = "South"},
                new Region {RegionCode = "RE - 002", RegionName = "North"},
                new Region {RegionCode = "RE - 003", RegionName = "Central"},
                new Region {RegionCode = "RE - 004", RegionName = "International"},
            };

            foreach (var region in regions)
                _context.Regions.Add(region);

            await _context.SaveChangesAsync();
        }

        private async Task SeedUserRegionsAsync()
        {
            // just for region manager
            var userRegions = new List<UserRegion>
            {
                new UserRegion { UserId = 2, RegionId = 1 },
                new UserRegion { UserId = 3, RegionId = 2 },
            };

            foreach (var ur in userRegions)
                _context.UserRegions.Add(ur);

            await _context.SaveChangesAsync();
        }

        private async Task SeedWarehouseAsync()
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    WarehouseCode = "WH - 001",
                    WarehouseName = "Warehouse HCM Base",
                    Address = "01 Le Duan, P.Ben Thanh, HCMC",
                    Description = "HCM Warehouse",
                    PhoneNumber = "1234567890",
                    RegionId = 1,
                },
                new Warehouse
                {
                    WarehouseCode = "WH - 002",
                    WarehouseName = "Warehouse HN Base",
                    Address = "01 Ho Xuan Huong, HN",
                    Description = "HN Warehouse",
                    PhoneNumber = "1234567890",
                    RegionId = 2,
                },
                new Warehouse
                {
                    WarehouseCode = "WH - 003",
                    WarehouseName = "Warehouse VT Base",
                    Address = "01 Hoang Hoa Tham, P.Vung Tau, HCMC",
                    Description = "VT Warehouse",
                    PhoneNumber = "1234567890",
                    RegionId = 1,
                },
            };

            foreach (var warehouse in warehouses)
                _context.Warehouses.Add(warehouse);

            await _context.SaveChangesAsync();
        }

        private async Task SeedUserWarehousesAsync()
        {
            // rule: RoleLevel > 2 (Regional Manager, Super Admin) are not included in this group
            var userWarehouses = new List<UserWarehouse>
            {
                new UserWarehouse { UserId = 4, WarehouseId = 1, IsWarehouseManager = true },
                new UserWarehouse { UserId = 4, WarehouseId = 2 },
                new UserWarehouse { UserId = 5, WarehouseId = 2, IsWarehouseManager = true },
                new UserWarehouse { UserId = 6, WarehouseId = 3, IsWarehouseManager = true },
                new UserWarehouse { UserId = 7, WarehouseId = 1 },
                new UserWarehouse { UserId = 8, WarehouseId = 2 },
                new UserWarehouse { UserId = 9, WarehouseId = 3 },
            };

            foreach (var uw in userWarehouses)
                _context.UserWarehouses.Add(uw);

            await _context.SaveChangesAsync();
        }

        private async Task SeedPermissionsAsync()
        {
            var permissions = new List<Permission>
            {
                #region Warehouse Permissions

                new Permission
                {
                    PermissionName = "WAREHOUSE_CREATE",
                    Module = "Warehouse",
                    Action = "Create",
                    Description = "Create new warehouse",
                    PermissionScope = PermissionScope.System
                },
                new Permission
                {
                    PermissionName = "WAREHOUSE_UPDATE",
                    Module = "Warehouse",
                    Action = "Update",
                    Description = "Update warehouse information .",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "WAREHOUSE_DELETE",
                    Module = "Warehouse",
                    Action = "Delete",
                    Description = "Soft delete warehouse .",
                    PermissionScope = PermissionScope.System
                },
                new Permission
                {
                    PermissionName = "WAREHOUSE_VIEW",
                    Module = "Warehouse",
                    Action = "View",
                    Description = "View warehouse",
                    PermissionScope = PermissionScope.Warehouse
                },

                #endregion

                #region Stock Transaction Permissions

                new Permission
                {
                    PermissionName = "STOCKTRANSACTION_VIEW",
                    Module = "StockTransaction",
                    Action = "View",
                    Description = "View Stock Transaction in warehouse. ",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "STOCKTRANSACTION_IMPORT",
                    Module = "StockTransaction",
                    Action = "Import",
                    Description = "Import products into warehouse .",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "STOCKTRANSACTION_EXPORT",
                    Module = "Warehouse",
                    Action = "Export",
                    Description = "Export products into warehouse .",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "STOCKTRANSACTION_TRANSFER",
                    Module = "StockTransaction",
                    Action = "Transfer",
                    Description = "Transfer product stock between warehouses. ",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "STOCKTRANSACTION_APPROVE_PROCESS",
                    Module = "StockTransaction",
                    Action = "Approve",
                    Description = "Approve Import / Export Process .",
                    PermissionScope = PermissionScope.Warehouse
                },

                #endregion

                #region Product Permmisions

                new Permission
                {
                    PermissionName = "PRODUCT_VIEW",
                    Module = "Product",
                    Action = "View",
                    Description = "View Product. ",
                    PermissionScope = PermissionScope.Warehouse
                },
                new Permission
                {
                    PermissionName = "PRODUCT_CREATE",
                    Module = "Product",
                    Action = "Create",
                    Description = "Create new product in application .",
                    PermissionScope = PermissionScope.System
                },
                new Permission
                {
                    PermissionName = "Product AccessPRODUCT_UPDATE_DELETE",
                    Module = "Product",
                    Action = "UpDel",
                    Description = "Update or Delete (soft delete) product in application .",
                    PermissionScope = PermissionScope.System
                },

                #endregion

            };

            foreach (var permission in permissions)
                _context.Permissions.Add(permission);

            await _context.SaveChangesAsync();
        }

        private async Task SeedRolePermissionsAsync()
        {
            // just have permission for Role < 2
            var rolePermissions = new List<RolePermission>
            {
                new RolePermission { RoleId = 4, PermissionId = 2 },
                new RolePermission { RoleId = 4, PermissionId = 4 },
                new RolePermission { RoleId = 4, PermissionId = 5 },
                new RolePermission { RoleId = 4, PermissionId = 6 },
                new RolePermission { RoleId = 4, PermissionId = 7 },
                new RolePermission { RoleId = 4, PermissionId = 8 },
                new RolePermission { RoleId = 4, PermissionId = 10 },
            };

            foreach (var rp in rolePermissions)
                _context.RolePermissions.Add(rp);

            await _context.SaveChangesAsync();
        }

        private async Task SeedCustomersAsync()
        {
            var customers = new List<Customer>
            {
                new Customer { CustomerCode = "CUS - 001", CustomerName = "Phoenix Dynamics", Address = "New York", PhoneNumber = "0900000001", Description = "Strategic Partner" },
                new Customer { CustomerCode = "CUS - 002", CustomerName = "Silverline Holdings", Address = "Los Angeles", PhoneNumber = "0900000002", Description = "Premium Client" },
                new Customer { CustomerCode = "CUS - 003", CustomerName = "NovaEdge Solutions", Address = "Chicago", PhoneNumber = "0900000003" },
                new Customer { CustomerCode = "CUS - 004", CustomerName = "Ironclad Ventures", Address = "Houston", PhoneNumber = "0900000004" },
                new Customer { CustomerCode = "CUS - 005", CustomerName = "BluePeak Industries", Address = "Seattle", PhoneNumber = "0900000005" },
                new Customer { CustomerCode = "CUS - 006", CustomerName = "Quantum Axis Corp", Address = "San Francisco", PhoneNumber = "0900000006" },
                new Customer { CustomerCode = "CUS - 007", CustomerName = "Velocity Group", Address = "Boston", PhoneNumber = "0900000007" },
                new Customer { CustomerCode = "CUS - 008", CustomerName = "Apex Horizon Ltd", Address = "Denver", PhoneNumber = "0900000008" },
                new Customer { CustomerCode = "CUS - 009", CustomerName = "TitanCore Enterprises", Address = "Miami", PhoneNumber = "0900000009" },
                new Customer { CustomerCode = "CUS - 010", CustomerName = "Eclipse Innovations", Address = "Atlanta", PhoneNumber = "0900000010" }
            };

            foreach (var c in customers)
                _context.Customers.Add(c);

            await _context.SaveChangesAsync();
        }

        private async Task SeedSuppliersAsync()
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierCode = "SUP - 001", SupplierName = "BlackForge Supply Co.", Address = "New York", PhoneNumber = "0911000001", Description = "Primary Materials Provider" },
                new Supplier { SupplierCode = "SUP - 002", SupplierName = "StormFront Logistics", Address = "Los Angeles", PhoneNumber = "0911000002" },
                new Supplier { SupplierCode = "SUP - 003", SupplierName = "IronPeak Distribution", Address = "Chicago", PhoneNumber = "0911000003" },
                new Supplier { SupplierCode = "SUP - 004", SupplierName = "Vanguard Industrial", Address = "Houston", PhoneNumber = "0911000004" },
                new Supplier { SupplierCode = "SUP - 005", SupplierName = "Titan Supply Chain", Address = "Seattle", PhoneNumber = "0911000005" },
                new Supplier { SupplierCode = "SUP - 006", SupplierName = "DarkMatter Exports", Address = "San Francisco", PhoneNumber = "0911000006" },
                new Supplier { SupplierCode = "SUP - 007", SupplierName = "QuantumTrade Global", Address = "Boston", PhoneNumber = "0911000007" },
                new Supplier { SupplierCode = "SUP - 008", SupplierName = "SilverStone Manufacturing", Address = "Denver", PhoneNumber = "0911000008" },
                new Supplier { SupplierCode = "SUP - 009", SupplierName = "CrimsonLine Wholesale", Address = "Miami", PhoneNumber = "0911000009" },
                new Supplier { SupplierCode = "SUP - 010", SupplierName = "Nebula Industrial Group", Address = "Atlanta", PhoneNumber = "0911000010" }
            };

            foreach (var c in suppliers)
                _context.Suppliers.Add(c);

            await _context.SaveChangesAsync();
        }

        private async Task SeedCategoriesAsync()
        {
            if (_context.Categories.Any())
                return;

            // ===== ROOT LEVEL =====
            var ingredients = new Category { Name = "Ingredients" };
            var beverages = new Category { Name = "Beverages" };
            var kitchenSupplies = new Category { Name = "Kitchen Supplies" };

            _context.Categories.AddRange(ingredients, beverages, kitchenSupplies);
            await _context.SaveChangesAsync(); // Generate Ids


            // ===== LEVEL 2 =====
            var meat = new Category { Name = "Meat", ParentId = ingredients.Id };
            var seafood = new Category { Name = "Seafood", ParentId = ingredients.Id };
            var vegetables = new Category { Name = "Vegetables", ParentId = ingredients.Id };
            var dairy = new Category { Name = "Dairy", ParentId = ingredients.Id };

            var wine = new Category { Name = "Wine", ParentId = beverages.Id };
            var beer = new Category { Name = "Beer", ParentId = beverages.Id };
            var softDrinks = new Category { Name = "Soft Drinks", ParentId = beverages.Id };

            var spices = new Category { Name = "Spices", ParentId = kitchenSupplies.Id };
            var sauces = new Category { Name = "Sauces", ParentId = kitchenSupplies.Id };

            _context.Categories.AddRange(
                meat, seafood, vegetables, dairy,
                wine, beer, softDrinks,
                spices, sauces
            );

            await _context.SaveChangesAsync();

            // ===== LEVEL 3 =====
            var beef = new Category { Name = "Beef", ParentId = meat.Id };
            var chicken = new Category { Name = "Chicken", ParentId = meat.Id };

            var redWine = new Category { Name = "Red Wine", ParentId = wine.Id };
            var whiteWine = new Category { Name = "White Wine", ParentId = wine.Id };

            var importedBeer = new Category { Name = "Imported Beer", ParentId = beer.Id };

            _context.Categories.AddRange(
                beef, chicken,
                redWine, whiteWine,
                importedBeer
            );

            await _context.SaveChangesAsync();
        }

        private async Task SeedUoMsAsync()
        {
            if (_context.UoMs.Any()) return;

            var uoms = new List<UoM>
            {
                new UoM { Name = "Kilogram" },
                new UoM { Name = "Gram" },
                new UoM { Name = "Liter" },
                new UoM { Name = "Milliliter" },
                new UoM { Name = "Bottle" },
                new UoM { Name = "Case" },
                new UoM { Name = "Piece" }
            };

            _context.UoMs.AddRange(uoms);
            await _context.SaveChangesAsync();
        }

        private async Task SeedProductsAsync()
        {
            if (_context.Products.Any()) return;

            var kg = await _context.UoMs.FirstAsync(x => x.Name == "Kilogram");
            var liter = await _context.UoMs.FirstAsync(x => x.Name == "Liter");
            var bottle = await _context.UoMs.FirstAsync(x => x.Name == "Bottle");

            var beef = await _context.Categories.FirstAsync(x => x.Name == "Beef");
            var chicken = await _context.Categories.FirstAsync(x => x.Name == "Chicken");
            var seafood = await _context.Categories.FirstAsync(x => x.Name == "Seafood");
            var vegetables = await _context.Categories.FirstAsync(x => x.Name == "Vegetables");
            var dairy = await _context.Categories.FirstAsync(x => x.Name == "Dairy");
            var redWine = await _context.Categories.FirstAsync(x => x.Name == "Red Wine");
            var beer = await _context.Categories.FirstAsync(x => x.Name == "Imported Beer");
            var softDrink = await _context.Categories.FirstAsync(x => x.Name == "Soft Drinks");
            var spices = await _context.Categories.FirstAsync(x => x.Name == "Spices");
            var sauces = await _context.Categories.FirstAsync(x => x.Name == "Sauces");

            var products = new List<Product>
            {
                new Product { Name="Beef Tenderloin", SKU="BF001", Barcode="8931234000017", CategoryId=beef.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Ribeye Steak", SKU="BF002", Barcode="8931234000024", CategoryId=beef.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Chicken Breast", SKU="CK001", Barcode="8931234000031", CategoryId=chicken.Id, BaseUoMId=kg.Id, MinStockLevel=10, IsPerishable=true },
                new Product { Name="Whole Chicken", SKU="CK002", Barcode="8931234000048", CategoryId=chicken.Id, BaseUoMId=kg.Id, MinStockLevel=8, IsPerishable=true },
                new Product { Name="Salmon Fillet", SKU="SF001", Barcode="8931234000055", CategoryId=seafood.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Shrimp", SKU="SF002", Barcode="8931234000062", CategoryId=seafood.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Broccoli", SKU="VG001", Barcode="8931234000079", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Carrot", SKU="VG002", Barcode="8931234000086", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Onion", SKU="VG003", Barcode="8931234000093", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=10, IsPerishable=true },
                new Product { Name="Milk", SKU="DY001", Barcode="8931234000109", CategoryId=dairy.Id, BaseUoMId=liter.Id, MinStockLevel=20, IsPerishable=true },
                new Product { Name="Cheddar Cheese", SKU="DY002", Barcode="8931234000116", CategoryId=dairy.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Butter", SKU="DY003", Barcode="8931234000123", CategoryId=dairy.Id, BaseUoMId=kg.Id, MinStockLevel=3, IsPerishable=true },
                new Product { Name="Cabernet Sauvignon", SKU="RW001", Barcode="8931234000130", CategoryId=redWine.Id, BaseUoMId=bottle.Id, MinStockLevel=24, IsPerishable=false },
                new Product { Name="Merlot", SKU="RW002", Barcode="8931234000147", CategoryId=redWine.Id, BaseUoMId=bottle.Id, MinStockLevel=24, IsPerishable=false },
                new Product { Name="Imported Lager Beer", SKU="BR001", Barcode="8931234000154", CategoryId=beer.Id, BaseUoMId=bottle.Id, MinStockLevel=48, IsPerishable=false },
                new Product { Name="Coca Cola", SKU="SD001", Barcode="8931234000161", CategoryId=softDrink.Id, BaseUoMId=bottle.Id, MinStockLevel=48, IsPerishable=false },
                new Product { Name="Black Pepper", SKU="SP001", Barcode="8931234000178", CategoryId=spices.Id, BaseUoMId=kg.Id, MinStockLevel=1, IsPerishable=false },
                new Product { Name="Salt", SKU="SP002", Barcode="8931234000185", CategoryId=spices.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=false },
                new Product { Name="Olive Oil", SKU="SC001", Barcode="8931234000192", CategoryId=sauces.Id, BaseUoMId=liter.Id, MinStockLevel=10, IsPerishable=false },
                new Product { Name="Tomato Sauce", SKU="SC002", Barcode="8931234000208", CategoryId=sauces.Id, BaseUoMId=liter.Id, MinStockLevel=10, IsPerishable=true }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }

        private async Task SeedProductConversionsAsync()
        {
            if (_context.ProductUoMConversions.Any()) return;

            var caseUom = await _context.UoMs.FirstAsync(x => x.Name == "Case");
            var bottle = await _context.UoMs.FirstAsync(x => x.Name == "Bottle");
            var gram = await _context.UoMs.FirstAsync(x => x.Name == "Gram");
            var kg = await _context.UoMs.FirstAsync(x => x.Name == "Kilogram");
            var ml = await _context.UoMs.FirstAsync(x => x.Name == "Milliliter");
            var liter = await _context.UoMs.FirstAsync(x => x.Name == "Liter");

            var beer = await _context.Products.FirstAsync(x => x.SKU == "BR001");
            var wine1 = await _context.Products.FirstAsync(x => x.SKU == "RW001");
            var wine2 = await _context.Products.FirstAsync(x => x.SKU == "RW002");
            var beef = await _context.Products.FirstAsync(x => x.SKU == "BF001");
            var milk = await _context.Products.FirstAsync(x => x.SKU == "DY001");

            var conversions = new List<ProductUoMConversion>
            {
                // Beer: 1 case = 12 bottle
                new ProductUoMConversion { ProductId=beer.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=12 },

                // Wine: 1 case = 6 bottle
                new ProductUoMConversion { ProductId=wine1.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=6 },
                new ProductUoMConversion { ProductId=wine2.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=6 },

                // Beef: 1 kg = 1000 g , 1 case = 10 kg
                new ProductUoMConversion { ProductId=beef.Id, FromUoMId=kg.Id, ToUoMId=gram.Id, Factor=1000 },
                new ProductUoMConversion { ProductId=beef.Id, FromUoMId=kg.Id, ToUoMId=caseUom.Id, Factor=10 },

                // Milk: 1 liter = 1000 ml
                new ProductUoMConversion { ProductId=milk.Id, FromUoMId=liter.Id, ToUoMId=ml.Id, Factor=1000 }
            };

            _context.ProductUoMConversions.AddRange(conversions);
            await _context.SaveChangesAsync();
        }

        private async Task SeedSupplierProductPricesAsync()
        {
            if (_context.SupplierProductPrices.Any()) return;

            var random = new Random();

            var supplierProductPrices = new List<SupplierProductPrice>();

            for (int supplierId = 1; supplierId <= 10; supplierId++)
            {
                // mỗi supplier có giá cho 5–10 sản phẩm
                var productCount = random.Next(5, 11);

                var productIds = Enumerable.Range(1, 20)
                                           .OrderBy(x => random.Next())
                                           .Take(productCount)
                                           .ToList();

                foreach (var productId in productIds)
                {
                    var price = random.Next(50_000, 500_000);

                    supplierProductPrices.Add(
                        new SupplierProductPrice
                        {
                            ProductId = productId,
                            SupplierId = supplierId,
                            UnitPrice = price,
                            EffectiveDate = DateTime.UtcNow.AddDays(-random.Next(1, 60))
                        }
                    );
                }
            }

            _context.SupplierProductPrices.AddRange(supplierProductPrices);
            await _context.SaveChangesAsync();
        }

        private async Task SeedPurchaseOrdersAsync()
        {
            if (_context.PurchaseOrders.Any()) return;

            var random = new Random();
            var purchaseOrders = new List<PurchaseOrder>();

            int sharedProductId = 1;

            for (int i = 1; i <= 10; i++)
            {
                var orderDate = DateTime.UtcNow.AddDays(-random.Next(1, 30));

                var po = new PurchaseOrder
                {
                    OrderNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{i:D3}",
                    SupplierId = random.Next(1, 6),
                    Status = PurchaseOrderStatus.Approved,
                    OrderDate = orderDate,
                    ApprovedDate = orderDate.AddDays(1),
                    Lines = new List<PurchaseOrderLine>()
                };

                int lineCount = random.Next(2, 5);

                var productIds = Enumerable.Range(1, 20)
                    .OrderBy(x => random.Next())
                    .Take(lineCount)
                    .ToList();

                // đảm bảo ProductId = 1 tồn tại
                if (!productIds.Contains(sharedProductId))
                    productIds[0] = sharedProductId;

                decimal totalAmount = 0;

                foreach (var productId in productIds)
                {
                    var orderedQty = random.Next(20, 100);
                    var unitPrice = random.Next(50_000, 500_000);

                    var line = new PurchaseOrderLine
                    {
                        ProductId = productId,
                        OrderedQty = orderedQty,
                        ReceivedQty = 0,
                        UnitPrice = unitPrice
                    };

                    totalAmount += orderedQty * unitPrice;
                    po.Lines.Add(line);
                }

                po.TotalAmount = totalAmount;
                purchaseOrders.Add(po);
            }

            _context.PurchaseOrders.AddRange(purchaseOrders);
            await _context.SaveChangesAsync();
        }

        private async Task SeedGoodsReceiptsAsync()
        {
            if (_context.GoodsReceipts.Any()) return;

            var random = new Random();

            var purchaseOrders = await _context.PurchaseOrders
                .Include(x => x.Lines)
                .ToListAsync();

            if (!purchaseOrders.Any()) return;

            int index = 1;

            foreach (var po in purchaseOrders.Take(5))
            {
                var receiptDate = po.OrderDate.AddDays(random.Next(2, 6));

                var receipt = new GoodsReceipt
                {
                    ReceiptNumber = $"GR-{DateTime.UtcNow:yyyyMMdd}-{index:D3}",
                    PurchaseOrderId = po.Id,
                    WarehouseId = random.Next(1, 3),
                    Status = ReceiptStatus.Posted,
                    ReceiptDate = receiptDate,
                    Lines = new List<GoodsReceiptLine>()
                };

                decimal totalAmount = 0;

                foreach (var poLine in po.Lines)
                {
                    var receivedQty = Math.Floor(
                        poLine.OrderedQty * (decimal)(0.7 + random.NextDouble() * 0.2));

                    if (receivedQty <= 0)
                        continue;

                    var unitCost = poLine.UnitPrice *
                        (decimal)(0.9 + random.NextDouble() * 0.2);

                    var line = new GoodsReceiptLine
                    {
                        PurchaseOrderId = po.Id,
                        ProductId = poLine.ProductId,
                        ReceivedQty = receivedQty,
                        UnitCost = Math.Round(unitCost, 2)
                    };

                    receipt.Lines.Add(line);

                    totalAmount += line.LineTotal;

                    poLine.ReceivedQty += receivedQty;
                }

                receipt.TotalAmount = totalAmount;

                _context.GoodsReceipts.Add(receipt);

                // SAVE để lấy receipt.Id
                await _context.SaveChangesAsync();

                var journalLines = new List<JournalEntryLine>();

                foreach (var line in receipt.Lines)
                {
                    // Cost Layer
                    var costLayer = new InventoryCostLayer
                    {
                        GoodsReceiptId = receipt.Id,
                        ProductId = line.ProductId,
                        WarehouseId = receipt.WarehouseId,
                        OriginalQty = line.ReceivedQty,
                        RemainingQty = line.ReceivedQty,
                        UnitCost = line.UnitCost,
                        ReceiptDate = receipt.ReceiptDate
                    };

                    _context.InventoryCostLayers.Add(costLayer);

                    // Ledger
                    var ledger = new InventoryLedger
                    {
                        ProductId = line.ProductId,
                        WarehouseId = receipt.WarehouseId,
                        TransactionType = InventoryTransactionType.Receipt,
                        ReferenceId = receipt.Id,
                        ReferenceType = "GoodsReceipt",
                        QuantityIn = line.ReceivedQty,
                        QuantityOut = 0,
                        UnitCost = line.UnitCost,
                        TotalCost = line.LineTotal,
                        TransactionDate = receipt.ReceiptDate
                    };

                    _context.InventoryLedgers.Add(ledger);

                    journalLines.Add(new JournalEntryLine
                    {
                        AccountId = (int)AccountCode.Inventory,
                        Debit = line.LineTotal,
                        Credit = 0
                    });
                }

                journalLines.Add(new JournalEntryLine
                {
                    AccountId = (int)AccountCode.Cash,
                    Debit = 0,
                    Credit = totalAmount
                });

                var journalEntry = new JournalEntry
                {
                    Reference = receipt.ReceiptNumber,
                    GoodsReceiptId = receipt.Id,
                    Lines = journalLines
                };

                _context.JournalEntries.Add(journalEntry);

                if (po.Lines.All(x => x.ReceivedQty >= x.OrderedQty))
                    po.Status = PurchaseOrderStatus.Completed;
                else
                    po.Status = PurchaseOrderStatus.PartiallyReceived;

                await _context.SaveChangesAsync();

                index++;
            }
        }

        private async Task SeedSalesOrdersAsync()
        {
            if (_context.SalesOrders.Any()) return;

            var random = new Random();

            var customers = await _context.Customers.ToListAsync();

            var layers = await _context.InventoryCostLayers
                .Where(x => x.RemainingQty > x.ReservedQty)
                .OrderBy(x => x.ReceiptDate)
                .ToListAsync();

            if (!customers.Any() || !layers.Any())
                return;

            int soIndex = 1;

            for (int i = 0; i < 5; i++)
            {
                var customer = customers[random.Next(customers.Count)];

                var order = new SalesOrder
                {
                    OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{soIndex:D3}",
                    CustomerId = customer.Id,
                    OrderDate = DateTime.UtcNow,
                    Status = SalesOrderStatus.Draft,
                    Lines = new List<SalesOrderLine>()
                };

                int rowNumber = 1;
                decimal totalAmount = 0;

                List<int> productIds;

                // SO đầu tiên bắt buộc product 1
                if (i == 0)
                {
                    productIds = new List<int> { 1 };
                }
                else
                {
                    productIds = layers
                        .Select(x => x.ProductId)
                        .Distinct()
                        .OrderBy(x => random.Next())
                        .Take(random.Next(1, 3))
                        .ToList();
                }

                foreach (var productId in productIds)
                {
                    decimal orderQty;

                    if (i == 0 && productId == 1)
                        orderQty = 15; // chắc chắn ăn nhiều layer
                    else
                        orderQty = random.Next(5, 20);

                    var fifoLayers = layers
                        .Where(x => x.ProductId == productId && x.RemainingQty > x.ReservedQty)
                        .OrderBy(x => x.ReceiptDate)
                        .ToList();

                    decimal remaining = orderQty;

                    foreach (var layer in fifoLayers)
                    {
                        if (remaining <= 0)
                            break;

                        var available = layer.RemainingQty - layer.ReservedQty;

                        if (available <= 0)
                            continue;

                        var takeQty = Math.Min(available, remaining);

                        remaining -= takeQty;

                        var line = new SalesOrderLine
                        {
                            ProductId = productId,
                            RowNumber = rowNumber,
                            OrderedQty = takeQty,
                            UnitPrice = layer.UnitCost
                        };

                        order.Lines.Add(line);

                        totalAmount += line.LineTotal;

                        var reservation = new InventoryReservation
                        {
                            ProductId = productId,
                            LayerId = layer.Id,
                            RowNumber = rowNumber,
                            ReservedQty = takeQty,
                            UnitPrice = layer.UnitCost
                        };

                        _context.InventoryReservations.Add(reservation);

                        layer.ReservedQty += takeQty;

                        rowNumber++;
                    }
                }

                order.TotalAmount = totalAmount;

                _context.SalesOrders.Add(order);

                soIndex++;
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedDeliveriesAsync()
        {
            if (_context.Deliveries.Any()) return;

            var random = new Random();

            var salesOrders = await _context.SalesOrders
                .Include(x => x.Lines)
                .ToListAsync();

            foreach (var so in salesOrders)
            {
                var delivery = new Delivery
                {
                    OrderNumber = $"DE-{DateTime.UtcNow:yyyyMMdd}-{so.Id:D3}",
                    SalesOrderId = so.Id,
                    DeliveryDate = DateTime.UtcNow,
                    Status = DeliveryStatus.Draft,
                    Lines = new List<DeliveryLine>()
                };

                decimal total = 0;

                foreach (var soLine in so.Lines)
                {
                    if (soLine.RemainingQty <= 0)
                        continue;

                    // giao khoảng 60% - 90%
                    var qty = Math.Min(
                        soLine.RemainingQty,
                        Math.Floor(soLine.OrderedQty * (decimal)(0.6 + random.NextDouble() * 0.3))
                    );

                    if (qty <= 0)
                        continue;

                    var line = new DeliveryLine
                    {
                        ProductId = soLine.ProductId,
                        RowNumber = soLine.RowNumber,
                        DeliveredQty = qty,
                        UnitPrice = soLine.UnitPrice
                    };

                    delivery.Lines.Add(line);

                    total += line.LineTotal;
                }

                delivery.TotalAmount = total;

                _context.Deliveries.Add(delivery);
            }

            await _context.SaveChangesAsync();

            // POST delivery
            var deliveries = await _context.Deliveries.ToListAsync();

            foreach (var delivery in deliveries)
            {
                await _deliveryService.PostAsync(delivery.Id);
            }
        }

        public async Task SeedInvoicesAsync()
        {
            if (_context.Invoices.Any()) return;

            var random = new Random();

            var deliveries = await _context.Deliveries
                .Include(d => d.Lines)
                .Where(d => d.Status == DeliveryStatus.Posted)
                .ToListAsync();

            foreach (var delivery in deliveries)
            {
                var lineDtos = new List<CreateInvoiceLineDto>();

                foreach (var line in delivery.Lines)
                {
                    var remaining = line.DeliveredQty - line.InvoicedQty;

                    if (remaining <= 0)
                        continue;

                    // invoice khoảng 50% - 100%
                    var qty = Math.Min(
                        remaining,
                        Math.Floor(line.DeliveredQty * (decimal)(0.5 + random.NextDouble() * 0.5))
                    );

                    if (qty <= 0)
                        continue;

                    lineDtos.Add(new CreateInvoiceLineDto
                    {
                        ProductId = line.ProductId,
                        RowNumber = line.RowNumber,
                        InvoiceQuantity = CF.GetInt(qty)
                    });
                }

                if (!lineDtos.Any())
                    continue;

                var dto = new CreateInvoiceDto
                {
                    DeliveryId = delivery.Id,
                    CreateInvoiceLineDtos = lineDtos
                };

                var result = await _invoiceService.CreateAsync(dto);

                if (result.IsSuccess)
                {
                    await _invoiceService.PostAsync(result.Data.Id);
                }
            }
        }
    }
}
