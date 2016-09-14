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
            CreateSlide(context);
        }

        private void CreateUser(TeduShopDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new TeduShopDbContext()));
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new TeduShopDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "tedu",
                Email = "tedu.international@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Technology Education",
            };

            manager.Create(user, "123456$");

            if (!roleManger.Roles.Any())
            {
                roleManger.Create(new IdentityRole { Name = "Admin" });
                roleManger.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("tedu.international@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
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

        private void CreateFooter(TeduShopDbContext context)
        {
            //if(context.Footers.Count(x=>x.ID == CommonConstants))
        }

        private void CreateSlide(TeduShopDbContext context)
        {
            if(context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide()
                    {
                        Name = "Slide 1",
                        DisplayOrder = 1,
                        Status = true, Url="#" ,
                        Image = "/Assets/client/images/bag.jpg",
                        Content = @"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </p>
                                <span class=""on-get"">GET NOW</span>"
                    },
                    new Slide()
                    {
                        Name = "Slide 2",
                        DisplayOrder = 2,
                        Status = true, Url="#" ,
                        Image = "/Assets/client/images/bag1.jpg",
                        Content = @"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </p>
                                <span class=""on-get"">GET NOW</span>"
                    }
                };
                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }
    }
}
