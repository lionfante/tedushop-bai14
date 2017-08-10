﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeduShop.Common;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;
        public ShoppingCartController(IProductService productService, 
            IOrderService orderService,
            ApplicationUserManager userManager)
        {
            _productService = productService;
            _orderService = orderService;
            _userManager = userManager;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if(Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }
            
            return View();
        }

        public ActionResult CheckOut()
        {
            if(Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if(cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if(product.Quantity == 0)
            {
                return Json(new { status = false, message = "Sản phẩm này hiện đang hết hàng" });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach(var item in cart)
                {
                    if(item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }

            }else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            Session[CommonConstants.SessionCart] = cart;
            return Json(new { status = true });
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new { data = cart, status = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new { data = user, status = true});
            }else
            {
                return Json(new {status = false });
            }
            
        }

        [HttpPost]
        public JsonResult CreateOrder(string orderViewModel)
        {
            var orderVm = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();
            orderNew.UpdateOrder(orderVm);

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                orderNew.CustomerId = userId;
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }
            if (isEnough)
            {
                _orderService.Create(orderNew, orderDetails);
                _productService.Save();
                return Json(new { status = true});
            }else
            {
                return Json(new { status = false, message = "Không đủ hàng." });
            }
            
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach(var item in cartSession)
            {
                foreach(var jitem in cartViewModel)
                {
                    if(item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }
            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new { status = true});
        }

        [HttpPost]
        public JsonResult Delete(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new { status = true });
        }
    }
}