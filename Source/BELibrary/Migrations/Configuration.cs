using BELibrary.Core.Utils;

namespace BELibrary.Migrations
{
    using BELibrary.DbContext;
    using BELibrary.Entity;
    using BELibrary.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<CinemaOnlineDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CinemaOnlineDbContext context)
        {
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                RoleEnum = RoleKey.Admin,
                Name = RoleKey.GetRole(RoleKey.Admin)
            };
            context.Roles.AddOrUpdate(c => c.RoleEnum, role);

            context.SaveChanges();
            string passwordFactory = VariableExtensions.DefautlPassword + VariableExtensions.KeyCryptor;
            string passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);
            context.Admins.AddOrUpdate(c => c.UserName, new Admin()
            {
                Id = Guid.NewGuid(),
                Name = "Quản Trị",
                PhoneNumber = "0973 011 012",
                UserName = "admin",
                Password = passwordCryptor,
                RoleId = role.Id,
                IsDelete = false,
                DeletetionTime = null,
                DeleterId = null
            });

            context.MovieDisplayTypes.AddOrUpdate(c => c.Name, new MovieDisplayType()
            {
                Id = Guid.NewGuid(),
                Name = "2D",
                IsDelete = false,
                DeletetionTime = null,
                DeleterId = null,
                Price = 0,
            });
            context.MovieDisplayTypes.AddOrUpdate(c => c.Name, new MovieDisplayType()
            {
                Id = Guid.NewGuid(),
                Name = "3D",
                IsDelete = false,
                DeletetionTime = null,
                DeleterId = null,
                Price = 10000
            });

            ///add seat type
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Thường",
                Color = "#0391D1",
                IsDelete = false,
                Price = 10000
            });
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Vip",
                Color = "#79AF3A",
                IsDelete = false,
                Price = 20000
            });
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Ghế đôi",
                Color = "#ED417B",
                IsDelete = false,
                Price = 35000
            });
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Lamour",
                Color = "#DB9A00",
                IsDelete = false,
                Price = 15000
            });
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Deluxe",
                Color = "#1F897F",
                IsDelete = false,
                Price = 20000
            });
            context.SeatTypes.AddOrUpdate(c => c.Name, new SeatType()
            {
                Id = Guid.NewGuid(),
                Name = "Premium",
                Color = "#953CA4",
                IsDelete = false,
                Price = 40000
            });

            //add movie day of week
            context.DaysOfWeeks.AddOrUpdate(c => c.Name,
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 2", IsDelete = false, Price = 0 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 3", IsDelete = false, Price = 0 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 4", IsDelete = false, Price = 0 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 5", IsDelete = false, Price = 0 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 6", IsDelete = false, Price = 10000 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Thứ 7", IsDelete = false, Price = 10000 },
                new DaysOfWeek() { Id = Guid.NewGuid(), Name = "Chủ nhật", IsDelete = false, Price = 10000 });
        }
    }
}