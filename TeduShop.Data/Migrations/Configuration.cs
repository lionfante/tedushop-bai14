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
            //CreateSlide(context);
            //CreatePage(context);
            CreateContactDetail(context);
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

        private void CreatePage(TeduShopDbContext context)
        {
            if(context.Pages.Count() == 0)
            {
                var page = new Page()
                {
                    Name = "Giới Thiệu",
                    Alias = "gioi-thieu",
                    Content = @"Lorem Ipsum chỉ đơn giản là một đoạn văn bản giả, được dùng vào việc trình bày và dàn trang phục vụ cho in ấn. 
                    Lorem Ipsum đã được sử dụng như một văn bản chuẩn cho ngành công nghiệp in ấn từ những năm 1500, khi một họa sĩ vô danh ghép nhiều đoạn văn bản với nhau để tạo thành một bản mẫu văn bản. 
                    Đoạn văn bản này không những đã tồn tại năm thế kỉ, mà khi được áp dụng vào tin học văn phòng, nội dung của nó vẫn không hề bị thay đổi. 
                    Nó đã được phổ biến trong những năm 1960 nhờ việc bán những bản giấy Letraset in những đoạn Lorem Ipsum, và gần đây hơn, được sử dụng trong các ứng dụng dàn trang, như Aldus PageMaker.",
                    Status = true

                };
                context.Pages.Add(page);
                context.SaveChanges();
            }
            
        }

        private void CreateContactDetail(TeduShopDbContext context)
        {
            if(context.ContactDetails.Count() == 0)
            {
                var page = new TeduShop.Model.Models.ContactDetails()
                {
                    Name = "Shop thời trang TEDU",
                    Address = "49 Trần Văn Đang",
                    Email = "lethienphu.1404@gmail.com",
                    Lat = 10.78345477892738,
                    Lng = 106.67793610581975,
                    Phone = "01677 011 757",
                    Website = "http://lethienphu.com.vn",
                    Order = "",
                    Status = true
                };
                context.ContactDetails.Add(page);
                context.SaveChanges();
            }
        }
    }
}
