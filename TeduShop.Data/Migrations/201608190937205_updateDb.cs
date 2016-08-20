namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDb : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VisitorStagistics", newName: "VisitorStatistics");
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        BirthDay = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.OrderDetails", "Quantitty", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Warranty", c => c.Int());
            AddColumn("dbo.SupportOnlines", "Yahoo", c => c.String(maxLength: 50));
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String());
            AlterColumn("dbo.Orders", "CreatedBy", c => c.String());
            AlterColumn("dbo.Products", "Alias", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Products", "MetaDescription", c => c.String(maxLength: 256));
            AlterColumn("dbo.ProductCategories", "Alias", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.ProductCategories", "MetaDescription", c => c.String(maxLength: 256));
            AlterColumn("dbo.Pages", "MetaDescription", c => c.String(maxLength: 256));
            AlterColumn("dbo.PostCategories", "ParentID", c => c.Int());
            AlterColumn("dbo.PostCategories", "Image", c => c.String(maxLength: 256));
            AlterColumn("dbo.PostCategories", "MetaDescription", c => c.String(maxLength: 256));
            AlterColumn("dbo.Posts", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Posts", "Alias", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.Posts", "Content", c => c.String());
            AlterColumn("dbo.Posts", "MetaDescription", c => c.String(maxLength: 256));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Slides", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.Slides", "Image", c => c.String(maxLength: 256));
            AlterColumn("dbo.Slides", "Url", c => c.String(maxLength: 256));
            AlterColumn("dbo.VisitorStatistics", "IPAddress", c => c.String(maxLength: 50));
            DropColumn("dbo.Orders", "UpdatedDate");
            DropColumn("dbo.Orders", "UpdatedBy");
            DropColumn("dbo.Orders", "MetaKeyword");
            DropColumn("dbo.Orders", "MetaDescription");
            DropColumn("dbo.Products", "Wanranty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Wanranty", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "MetaDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.Orders", "MetaKeyword", c => c.String(maxLength: 256));
            AddColumn("dbo.Orders", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.Orders", "UpdatedDate", c => c.DateTime());
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            AlterColumn("dbo.VisitorStatistics", "IPAddress", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Slides", "Url", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Slides", "Image", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Slides", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Posts", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Posts", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "Alias", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.Posts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PostCategories", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.PostCategories", "Image", c => c.String(maxLength: 500));
            AlterColumn("dbo.PostCategories", "ParentID", c => c.Int(nullable: false));
            AlterColumn("dbo.Pages", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.ProductCategories", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.ProductCategories", "Alias", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.Products", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Products", "Alias", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.Orders", "CreatedBy", c => c.String(maxLength: 256));
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String(maxLength: 256));
            DropColumn("dbo.SupportOnlines", "Yahoo");
            DropColumn("dbo.Products", "Warranty");
            DropColumn("dbo.OrderDetails", "Quantitty");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            RenameTable(name: "dbo.VisitorStatistics", newName: "VisitorStagistics");
        }
    }
}
