namespace BELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 15),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 250),
                        RoleId = c.Guid(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        RoleEnum = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Banner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CinemaRoom",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 20),
                        RowQuantity = c.Int(nullable: false),
                        Area = c.Double(),
                        SeatQuantity = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        SoundQuality = c.String(maxLength: 20),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seat",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Row = c.String(maxLength: 2),
                        Col = c.String(maxLength: 2),
                        Status = c.Int(nullable: false),
                        Color = c.String(),
                        Price = c.Double(nullable: false),
                        SeatTypeId = c.Guid(),
                        CinemaRoomId = c.Guid(),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CinemaRoom", t => t.CinemaRoomId)
                .ForeignKey("dbo.SeatType", t => t.SeatTypeId)
                .Index(t => t.SeatTypeId)
                .Index(t => t.CinemaRoomId);
            
            CreateTable(
                "dbo.SeatType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 15),
                        Price = c.Double(nullable: false),
                        Color = c.String(maxLength: 10),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        FilmId = c.Guid(nullable: false),
                        Detail = c.String(),
                        CreationTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Film", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FilmId);
            
            CreateTable(
                "dbo.Film",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 250),
                        TrailerLink = c.String(),
                        Image = c.String(),
                        Actors = c.String(),
                        AgeRestriction = c.Int(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                        Directors = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        Time = c.Time(precision: 7),
                        BannerUrl = c.String(maxLength: 250),
                        TimeComing = c.String(maxLength: 50),
                        Detail = c.String(),
                        Price = c.Double(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmMovieDisplayType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilmId = c.Guid(nullable: false),
                        MovieDisplayTypeId = c.Guid(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Film", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.MovieDisplayType", t => t.MovieDisplayTypeId, cascadeDelete: true)
                .Index(t => t.FilmId)
                .Index(t => t.MovieDisplayTypeId);
            
            CreateTable(
                "dbo.MovieDisplayType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 20),
                        Price = c.Double(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieCalendar",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FilmId = c.Guid(nullable: false),
                        DaysOfWeekId = c.Guid(nullable: false),
                        TimeFrameId = c.Guid(nullable: false),
                        CinemaRoomId = c.Guid(nullable: false),
                        MovieDisplayTypeId = c.Guid(nullable: false),
                        Price = c.Double(nullable: false),
                        StartWeek = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                        DateTimeDetail = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CinemaRoom", t => t.CinemaRoomId, cascadeDelete: true)
                .ForeignKey("dbo.DaysOfWeek", t => t.DaysOfWeekId, cascadeDelete: true)
                .ForeignKey("dbo.Film", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.MovieDisplayType", t => t.MovieDisplayTypeId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrame", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.FilmId)
                .Index(t => t.DaysOfWeekId)
                .Index(t => t.TimeFrameId)
                .Index(t => t.CinemaRoomId)
                .Index(t => t.MovieDisplayTypeId);
            
            CreateTable(
                "dbo.DaysOfWeek",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 30),
                        Price = c.Double(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeFrame",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        Price = c.Double(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmMovieType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilmId = c.Guid(nullable: false),
                        MovieTypeId = c.Guid(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Film", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.MovieType", t => t.MovieTypeId, cascadeDelete: true)
                .Index(t => t.FilmId)
                .Index(t => t.MovieTypeId);
            
            CreateTable(
                "dbo.MovieType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 30),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(maxLength: 50),
                        Gender = c.Boolean(nullable: false),
                        IdentityCard = c.String(maxLength: 15),
                        Email = c.String(maxLength: 30),
                        DateOfBirth = c.DateTime(nullable: false),
                        Phone = c.String(maxLength: 15),
                        Username = c.String(maxLength: 50),
                        Password = c.String(maxLength: 100),
                        LinkAvata = c.String(maxLength: 250),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        PromotionFilmId = c.Guid(),
                        PromotionFilmPrice = c.Double(),
                        PromotionToppingId = c.Guid(),
                        PromotionToppingPrice = c.Double(),
                        MovieCalendarId = c.Guid(nullable: false),
                        FilmName = c.String(),
                        FilmType = c.String(),
                        RoomName = c.String(),
                        FilmId = c.Guid(nullable: false),
                        FilmPrice = c.Double(nullable: false),
                        DayOfWeekPrice = c.Double(nullable: false),
                        TimeFramePrice = c.Double(nullable: false),
                        Time = c.DateTime(nullable: false),
                        MovieDisplayTypePrice = c.Double(nullable: false),
                        TicketNumber = c.String(),
                        CreationTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TotalPrice = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Credits = c.String(maxLength: 50),
                        TicketQuantity = c.Int(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeleteTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieCalendar", t => t.MovieCalendarId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MovieCalendarId);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        SeatId = c.Guid(nullable: false),
                        SeatName = c.String(maxLength: 4),
                        IsDelete = c.Boolean(nullable: false),
                        DeleteTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                        SeatPrice = c.Double(nullable: false),
                        FilmPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Seat", t => t.SeatId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.SeatId);
            
            CreateTable(
                "dbo.ToppingOrderDetail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        ToppingId = c.Guid(nullable: false),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        DeleteTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Topping", t => t.ToppingId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ToppingId);
            
            CreateTable(
                "dbo.Topping",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Price = c.Double(nullable: false),
                        ImageUrl = c.String(),
                        KindOfTopping = c.String(maxLength: 50),
                        KindOfToppingEnum = c.Int(),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gallery",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        Url = c.String(maxLength: 250),
                        UrlThumb = c.String(maxLength: 250),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Detail = c.String(unicode: false, storeType: "text"),
                        CreationTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreaterId = c.Guid(nullable: false),
                        BannerUrl = c.String(maxLength: 250),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admin", t => t.CreaterId, cascadeDelete: true)
                .Index(t => t.CreaterId);
            
            CreateTable(
                "dbo.Promotion",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreationTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreaterId = c.Guid(),
                        IsDelete = c.Boolean(nullable: false),
                        DeletetionTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeleterId = c.Guid(),
                        KindOfPromotion = c.String(maxLength: 50),
                        KindOfPromotionEnum = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PromotionPercent = c.Double(),
                        IsFilm = c.Boolean(nullable: false),
                        FilmId = c.Guid(),
                        Code = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ToppingId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Film", t => t.FilmId)
                .ForeignKey("dbo.Topping", t => t.ToppingId)
                .Index(t => t.FilmId)
                .Index(t => t.ToppingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promotion", "ToppingId", "dbo.Topping");
            DropForeignKey("dbo.Promotion", "FilmId", "dbo.Film");
            DropForeignKey("dbo.News", "CreaterId", "dbo.Admin");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.ToppingOrderDetail", "ToppingId", "dbo.Topping");
            DropForeignKey("dbo.ToppingOrderDetail", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDetail", "SeatId", "dbo.Seat");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "MovieCalendarId", "dbo.MovieCalendar");
            DropForeignKey("dbo.Comment", "UserId", "dbo.Users");
            DropForeignKey("dbo.FilmMovieType", "MovieTypeId", "dbo.MovieType");
            DropForeignKey("dbo.FilmMovieType", "FilmId", "dbo.Film");
            DropForeignKey("dbo.MovieCalendar", "TimeFrameId", "dbo.TimeFrame");
            DropForeignKey("dbo.MovieCalendar", "MovieDisplayTypeId", "dbo.MovieDisplayType");
            DropForeignKey("dbo.MovieCalendar", "FilmId", "dbo.Film");
            DropForeignKey("dbo.MovieCalendar", "DaysOfWeekId", "dbo.DaysOfWeek");
            DropForeignKey("dbo.MovieCalendar", "CinemaRoomId", "dbo.CinemaRoom");
            DropForeignKey("dbo.FilmMovieDisplayType", "MovieDisplayTypeId", "dbo.MovieDisplayType");
            DropForeignKey("dbo.FilmMovieDisplayType", "FilmId", "dbo.Film");
            DropForeignKey("dbo.Comment", "FilmId", "dbo.Film");
            DropForeignKey("dbo.Seat", "SeatTypeId", "dbo.SeatType");
            DropForeignKey("dbo.Seat", "CinemaRoomId", "dbo.CinemaRoom");
            DropForeignKey("dbo.Admin", "RoleId", "dbo.Role");
            DropIndex("dbo.Promotion", new[] { "ToppingId" });
            DropIndex("dbo.Promotion", new[] { "FilmId" });
            DropIndex("dbo.News", new[] { "CreaterId" });
            DropIndex("dbo.ToppingOrderDetail", new[] { "ToppingId" });
            DropIndex("dbo.ToppingOrderDetail", new[] { "OrderId" });
            DropIndex("dbo.OrderDetail", new[] { "SeatId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "MovieCalendarId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.FilmMovieType", new[] { "MovieTypeId" });
            DropIndex("dbo.FilmMovieType", new[] { "FilmId" });
            DropIndex("dbo.MovieCalendar", new[] { "MovieDisplayTypeId" });
            DropIndex("dbo.MovieCalendar", new[] { "CinemaRoomId" });
            DropIndex("dbo.MovieCalendar", new[] { "TimeFrameId" });
            DropIndex("dbo.MovieCalendar", new[] { "DaysOfWeekId" });
            DropIndex("dbo.MovieCalendar", new[] { "FilmId" });
            DropIndex("dbo.FilmMovieDisplayType", new[] { "MovieDisplayTypeId" });
            DropIndex("dbo.FilmMovieDisplayType", new[] { "FilmId" });
            DropIndex("dbo.Comment", new[] { "FilmId" });
            DropIndex("dbo.Comment", new[] { "UserId" });
            DropIndex("dbo.Seat", new[] { "CinemaRoomId" });
            DropIndex("dbo.Seat", new[] { "SeatTypeId" });
            DropIndex("dbo.Admin", new[] { "RoleId" });
            DropTable("dbo.Promotion");
            DropTable("dbo.News");
            DropTable("dbo.Gallery");
            DropTable("dbo.Topping");
            DropTable("dbo.ToppingOrderDetail");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.MovieType");
            DropTable("dbo.FilmMovieType");
            DropTable("dbo.TimeFrame");
            DropTable("dbo.DaysOfWeek");
            DropTable("dbo.MovieCalendar");
            DropTable("dbo.MovieDisplayType");
            DropTable("dbo.FilmMovieDisplayType");
            DropTable("dbo.Film");
            DropTable("dbo.Comment");
            DropTable("dbo.SeatType");
            DropTable("dbo.Seat");
            DropTable("dbo.CinemaRoom");
            DropTable("dbo.Banner");
            DropTable("dbo.Role");
            DropTable("dbo.Admin");
        }
    }
}
