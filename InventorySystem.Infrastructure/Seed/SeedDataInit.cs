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

            if (!await context.Warehouses.AnyAsync())
                await SeedWarehouseAsync(context);
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
                    RoleName = "Super Admin",
                    Description = "Full Access anywhere in application.",
                    RoleLevel = RoleLevel.SuperAdmin,
                },
                new Role
                {
                    RoleName = "Regional Manager",
                    Description = "Full Access any warehouse in area they manage.",
                    RoleLevel = RoleLevel.RegionalManager,
                },
                new Role
                {
                    RoleName = "Warehouse Manager",
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

        private static async Task SeedWarehouseAsync(ApplicationDbContext context)
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    WarehouseCode = "WH - 001",
                    WarehouseName = "Warehouse HCM Base",
                    Address = "01 Le Duan, P.Ben Thanh, HCMC",
                    Region = WarehouseRegion.South,
                    Description = "HCM Warehouse",
                    PhoneNumber = "1234567890",
                    ManagerId = 4,
                },
                new Warehouse
                {
                    WarehouseCode = "WH - 002",
                    WarehouseName = "Warehouse HN Base",
                    Address = "01 Ho Xuan Huong, HN",
                    Region = WarehouseRegion.North,
                    Description = "HN Warehouse",
                    PhoneNumber = "1234567890",
                    ManagerId = 5,
                },
                new Warehouse
                {
                    WarehouseCode = "WH - 003",
                    WarehouseName = "Warehouse VT Base",
                    Address = "01 Hoang Hoa Tham, P.Vung Tau, HCMC",
                    Region = WarehouseRegion.South,
                    Description = "VT Warehouse",
                    PhoneNumber = "1234567890",
                    ManagerId = 6,
                },
            };

            foreach (var warehouse in warehouses)
                context.Warehouses.Add(warehouse);

            await context.SaveChangesAsync();
        }
    }
}
