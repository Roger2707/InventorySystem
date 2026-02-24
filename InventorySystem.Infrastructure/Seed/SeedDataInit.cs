using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Entities.Identity;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventorySystem.Infrastructure.Seed
{
    public static class SeedDataInit
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var hasher = serviceProvider.GetRequiredService<IPasswordHasher>();

            if (!await context.Users.AnyAsync())
                await SeedUserAsyc(context, hasher);

            if (!await context.Roles.AnyAsync())
                await SeedRoleAsync(context);

            if(await context.Users.AnyAsync() && await context.Roles.AnyAsync() && !await context.UserRoles.AnyAsync())
                await SeedUserRoleAsync(context);

            if (!await context.Regions.AnyAsync())
                await SeedRegionAsync(context);

            if (await context.Users.AnyAsync() && await context.Regions.AnyAsync() && !await context.UserRegions.AnyAsync())
                await SeedUserRegionsAsync(context);

            if (!await context.Warehouses.AnyAsync())
                await SeedWarehouseAsync(context);

            if (await context.Users.AnyAsync() && await context.Warehouses.AnyAsync() && !await context.UserWarehouses.AnyAsync())
                await SeedUserWarehousesAsync(context);

            if (!await context.Permissions.AnyAsync())
                await SeedPermissionsAsync(context);

            if (await context.Roles.AnyAsync() && await context.Permissions.AnyAsync() && !await context.RolePermissions.AnyAsync())
                await SeedRolePermissionsAsync(context);

            if (!await context.Customers.AnyAsync())
                await SeedCustomersAsync(context);

            if (!await context.Suppliers.AnyAsync())
                await SeedSuppliersAsync(context);

            if (!await context.Categories.AnyAsync())
                await SeedCategoriesAsync(context);

            if (!await context.UoMs.AnyAsync())
                await SeedUoMsAsync(context);

            if (!await context.Products.AnyAsync())
                await SeedProductsAsync(context);

            if (await context.Products.AnyAsync() && await context.UoMs.AnyAsync() && !await context.ProductUoMConversions.AnyAsync())
                await SeedProductConversionsAsync(context);
        }

        private static async Task SeedUserAsyc(ApplicationDbContext context, IPasswordHasher hasher)
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
                    PasswordHash = hasher.HashPassword("Rogersa@123"),
                },
                new User
                {
                    Username = "greatorm",
                    FullName = "Greato RM",
                    Email = "greatorm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "03 PDP, P.Cau Kieu, HCMC",
                    PasswordHash = hasher.HashPassword("Greatorm@123"),
                },
                new User
                {
                    Username = "baoanrm",
                    FullName = "Bao An RM",
                    Email = "baoanrm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "05 Van Kiep, P.Phu Nhuan, HCMC",
                    PasswordHash = hasher.HashPassword("Baoanrm@123"),
                },
                new User
                {
                    Username = "gapuwm",
                    FullName = "Gapu Truong WM",
                    Email = "gapuwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "07 Tan Dinh, P.Tan Dinh, HCMC",
                    PasswordHash = hasher.HashPassword("Gapuwm@123"),
                },
                new User
                {
                    Username = "nguyenwm",
                    FullName = "Truong Nguyen WM",
                    Email = "nguyenwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "09 Phan Dang Luu, P.Thanh My Tay, HCMC",
                    PasswordHash = hasher.HashPassword("Nguyenwm@123"),
                },
                new User
                {
                    Username = "duthwm",
                    FullName = "Duc Thang WM",
                    Email = "duthwm@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "11 Quang Trung, P.Go Vap, HCMC",
                    PasswordHash = hasher.HashPassword("Duthwm@123"),
                },
                new User
                {
                    Username = "quincyst",
                    FullName = "Quincy ST",
                    Email = "quincyst@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "13 Pham Van Dong, P.Thu Duc, HCMC",
                    PasswordHash = hasher.HashPassword("Quincyst@123"),
                },
                new User
                {
                    Username = "alliest",
                    FullName = "Thuy Hang ST",
                    Email = "alliest@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "15 Ben Van Don, P.Pham Ngu Lao, HCMC",
                    PasswordHash = hasher.HashPassword("Alliest@123"),
                },
                new User
                {
                    Username = "mist",
                    FullName = "Nhu Quynh ST",
                    Email = "mist@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "17 Hoang Hoa Tham, P.Bien Hoa, HCMC",
                    PasswordHash = hasher.HashPassword("Mist@123"),
                },
            }; 

            foreach(var user in users)
                context.Users.Add(user);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRoleAsync(ApplicationDbContext context)
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
                context.Roles.Add(role);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUserRoleAsync(ApplicationDbContext context)
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

            foreach(var ur in userRoles)
                context.UserRoles.Add(ur);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRegionAsync(ApplicationDbContext context)
        {
            var regions = new List<Region>
            {
                new Region {RegionCode = "RE - 001", RegionName = "South"},
                new Region {RegionCode = "RE - 002", RegionName = "North"},
                new Region {RegionCode = "RE - 003", RegionName = "Central"},
                new Region {RegionCode = "RE - 004", RegionName = "International"},
            };

            foreach (var region in regions) 
                context.Regions.Add(region);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUserRegionsAsync(ApplicationDbContext context)
        {
            // just for region manager
            var userRegions = new List<UserRegion>
            {
                new UserRegion { UserId = 2, RegionId = 1 },
                new UserRegion { UserId = 3, RegionId = 2 },
            };

            foreach (var ur in userRegions)
                context.UserRegions.Add(ur);

            await context.SaveChangesAsync();
        }

        private static async Task SeedWarehouseAsync(ApplicationDbContext context)
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
                context.Warehouses.Add(warehouse);

            await context.SaveChangesAsync();
        }

        private static async Task SeedUserWarehousesAsync(ApplicationDbContext context)
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
                context.UserWarehouses.Add(uw);

            await context.SaveChangesAsync();
        }

        private static async Task SeedPermissionsAsync(ApplicationDbContext context)
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

            foreach(var permission in permissions)
                context.Permissions.Add(permission);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolePermissionsAsync(ApplicationDbContext context)
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

            foreach(var rp in rolePermissions)
                context.RolePermissions.Add(rp);

            await context.SaveChangesAsync();
        }

        private static async Task SeedCustomersAsync(ApplicationDbContext context)
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
                context.Customers.Add(c);

            await context.SaveChangesAsync();
        }

        private static async Task SeedSuppliersAsync(ApplicationDbContext context)
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
                context.Suppliers.Add(c);

            await context.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            if (context.Categories.Any())
                return;

            // ===== ROOT LEVEL =====
            var ingredients = new Category { Name = "Ingredients" };
            var beverages = new Category { Name = "Beverages" };
            var kitchenSupplies = new Category { Name = "Kitchen Supplies" };

            context.Categories.AddRange(ingredients, beverages, kitchenSupplies);
            await context.SaveChangesAsync(); // Generate Ids


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

            context.Categories.AddRange(
                meat, seafood, vegetables, dairy,
                wine, beer, softDrinks,
                spices, sauces
            );

            await context.SaveChangesAsync();

            // ===== LEVEL 3 =====
            var beef = new Category { Name = "Beef", ParentId = meat.Id };
            var chicken = new Category { Name = "Chicken", ParentId = meat.Id };

            var redWine = new Category { Name = "Red Wine", ParentId = wine.Id };
            var whiteWine = new Category { Name = "White Wine", ParentId = wine.Id };

            var importedBeer = new Category { Name = "Imported Beer", ParentId = beer.Id };

            context.Categories.AddRange(
                beef, chicken,
                redWine, whiteWine,
                importedBeer
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedUoMsAsync(ApplicationDbContext context)
        {
            if (context.UoMs.Any()) return;

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

            context.UoMs.AddRange(uoms);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            if (context.Products.Any()) return;

            var kg = await context.UoMs.FirstAsync(x => x.Name == "Kilogram");
            var liter = await context.UoMs.FirstAsync(x => x.Name == "Liter");
            var bottle = await context.UoMs.FirstAsync(x => x.Name == "Bottle");

            var beef = await context.Categories.FirstAsync(x => x.Name == "Beef");
            var chicken = await context.Categories.FirstAsync(x => x.Name == "Chicken");
            var seafood = await context.Categories.FirstAsync(x => x.Name == "Seafood");
            var vegetables = await context.Categories.FirstAsync(x => x.Name == "Vegetables");
            var dairy = await context.Categories.FirstAsync(x => x.Name == "Dairy");
            var redWine = await context.Categories.FirstAsync(x => x.Name == "Red Wine");
            var beer = await context.Categories.FirstAsync(x => x.Name == "Imported Beer");
            var softDrink = await context.Categories.FirstAsync(x => x.Name == "Soft Drinks");
            var spices = await context.Categories.FirstAsync(x => x.Name == "Spices");
            var sauces = await context.Categories.FirstAsync(x => x.Name == "Sauces");

            var products = new List<Product>
            {
                new Product { Name="Beef Tenderloin", SKU="BF001", CategoryId=beef.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Ribeye Steak", SKU="BF002", CategoryId=beef.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Chicken Breast", SKU="CK001", CategoryId=chicken.Id, BaseUoMId=kg.Id, MinStockLevel=10, IsPerishable=true },
                new Product { Name="Whole Chicken", SKU="CK002", CategoryId=chicken.Id, BaseUoMId=kg.Id, MinStockLevel=8, IsPerishable=true },
                new Product { Name="Salmon Fillet", SKU="SF001", CategoryId=seafood.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Shrimp", SKU="SF002", CategoryId=seafood.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Broccoli", SKU="VG001", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Carrot", SKU="VG002", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Onion", SKU="VG003", CategoryId=vegetables.Id, BaseUoMId=kg.Id, MinStockLevel=10, IsPerishable=true },
                new Product { Name="Milk", SKU="DY001", CategoryId=dairy.Id, BaseUoMId=liter.Id, MinStockLevel=20, IsPerishable=true },
                new Product { Name="Cheddar Cheese", SKU="DY002", CategoryId=dairy.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=true },
                new Product { Name="Butter", SKU="DY003", CategoryId=dairy.Id, BaseUoMId=kg.Id, MinStockLevel=3, IsPerishable=true },
                new Product { Name="Cabernet Sauvignon", SKU="RW001", CategoryId=redWine.Id, BaseUoMId=bottle.Id, MinStockLevel=24, IsPerishable=false },
                new Product { Name="Merlot", SKU="RW002", CategoryId=redWine.Id, BaseUoMId=bottle.Id, MinStockLevel=24, IsPerishable=false },
                new Product { Name="Imported Lager Beer", SKU="BR001", CategoryId=beer.Id, BaseUoMId=bottle.Id, MinStockLevel=48, IsPerishable=false },
                new Product { Name="Coca Cola", SKU="SD001", CategoryId=softDrink.Id, BaseUoMId=bottle.Id, MinStockLevel=48, IsPerishable=false },
                new Product { Name="Black Pepper", SKU="SP001", CategoryId=spices.Id, BaseUoMId=kg.Id, MinStockLevel=1, IsPerishable=false },
                new Product { Name="Salt", SKU="SP002", CategoryId=spices.Id, BaseUoMId=kg.Id, MinStockLevel=5, IsPerishable=false },
                new Product { Name="Olive Oil", SKU="SC001", CategoryId=sauces.Id, BaseUoMId=liter.Id, MinStockLevel=10, IsPerishable=false },
                new Product { Name="Tomato Sauce", SKU="SC002", CategoryId=sauces.Id, BaseUoMId=liter.Id, MinStockLevel=10, IsPerishable=true }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductConversionsAsync(ApplicationDbContext context)
        {
            if (context.ProductUoMConversions.Any()) return;

            var caseUom = await context.UoMs.FirstAsync(x => x.Name == "Case");
            var bottle = await context.UoMs.FirstAsync(x => x.Name == "Bottle");
            var gram = await context.UoMs.FirstAsync(x => x.Name == "Gram");
            var kg = await context.UoMs.FirstAsync(x => x.Name == "Kilogram");
            var ml = await context.UoMs.FirstAsync(x => x.Name == "Milliliter");
            var liter = await context.UoMs.FirstAsync(x => x.Name == "Liter");

            var beer = await context.Products.FirstAsync(x => x.SKU == "BR001");
            var wine1 = await context.Products.FirstAsync(x => x.SKU == "RW001");
            var wine2 = await context.Products.FirstAsync(x => x.SKU == "RW002");
            var beef = await context.Products.FirstAsync(x => x.SKU == "BF001");
            var milk = await context.Products.FirstAsync(x => x.SKU == "DY001");

            var conversions = new List<ProductUoMConversion>
            {
                // Beer: 1 case = 12 bottle
                new ProductUoMConversion { ProductId=beer.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=12 },

                // Wine: 1 case = 6 bottle
                new ProductUoMConversion { ProductId=wine1.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=6 },
                new ProductUoMConversion { ProductId=wine2.Id, FromUoMId=caseUom.Id, ToUoMId=bottle.Id, Factor=6 },

                // Beef: 1 kg = 1000 g
                new ProductUoMConversion { ProductId=beef.Id, FromUoMId=kg.Id, ToUoMId=gram.Id, Factor=1000 },

                // Milk: 1 liter = 1000 ml
                new ProductUoMConversion { ProductId=milk.Id, FromUoMId=liter.Id, ToUoMId=ml.Id, Factor=1000 }
            };

            context.ProductUoMConversions.AddRange(conversions);
            await context.SaveChangesAsync();
        }
    }
}
