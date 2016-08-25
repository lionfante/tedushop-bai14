namespace TeduShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TeduShop.Data.TeduShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TeduShop.Data.TeduShopDbContext context)
        {
            CreateProductCategorySample(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new TeduShopDbContext()));
            //var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new TeduShopDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "tedu",
            //    Email = "tedu.international@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    FullName = "Technology Education",
            //};

            //manager.Create(user, "123456$");

            //if (!roleManger.Roles.Any())
            //{
            //    roleManger.Create(new IdentityRole { Name = "Admin" });
            //    roleManger.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("tedu.international@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }

        private void CreateProductCategorySample(TeduShop.Data.TeduShopDbContext context)
        {
            List<ProductCategory> listProductCategory = new List<ProductCategory>()
            {
                new ProductCategory
                {
                    Name = "Điện Lạnh",
                    Alias = "dien-lanh",
                    Status = true

                },
                new ProductCategory
                {
                    Name = "Viễn Thông",
                    Alias = "vien-thong",
                    Status = true

                },
                new ProductCategory
                {
                    Name = "Đồ Gia Dụng",
                    Alias = "do-gia-dung",
                    Status = true

                },
                new ProductCategory
                {
                    Name = "Mỹ Phẩm",
                    Alias = "my-pham",
                    Status = true

                },
            };
            context.ProductCategories.AddRange(listProductCategory);
            context.SaveChanges();
        }
    }
}
