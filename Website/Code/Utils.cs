﻿// Copyright: 2012 
// File name: Utils.cs
// Solution: iShopSolution
// Project: Website

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Xml.Linq;
using Website.Code.Entity;
using Website.ShopService;
using System.Linq;

namespace Website.Code
{
    public static class Utils
    {
        //Service client
        public static ShopServiceClient Service
        {
            get { return new ShopServiceClient(); }
        }

        private static readonly string _filename = HttpContext.Current.Server.MapPath("~/App_Data/data.xml");
        private static readonly string _fileConfig = HttpContext.Current.Server.MapPath("~/App_Data/config.web");

        public static bool SaveShoppingCart(Cart cart)
        {

            XDocument xdoc;
            cart.Id = NextId();
            var eCart = cart.ConvertToXElementCart();

            if (!File.Exists(_filename))
            {
                xdoc = new XDocument(new XDeclaration("1.0", "utf-8", ""), new XElement("shoppingcart", eCart));

                return Extension.SaveXml(xdoc, _filename);
            }

            xdoc = Extension.LoadXml(_filename);

            var root = xdoc.Element("shoppingcart");
            if (root == null) return false;
            if (root.ToString().Equals("<shoppingcart />"))
            {
                root.Add(eCart);
            }
            else
            {
                var last = root.Descendants("cart").Last();
                last.AddAfterSelf(eCart);
            }

            return Extension.SaveXml(xdoc, _filename);

        }

        public static void UpdateStatusOrder(int id, int deliveryId)
        {
            if (!File.Exists(_filename)) return;

            var xdoc = Extension.LoadXml(_filename);
            var root = xdoc.Element("shoppingcart");
            if (root == null || root.ToString().Equals("<shoppingcart />")) return;
            var elm = root.Descendants("cart").FirstOrDefault(e => Convert.ToInt32(e.GetElementValue("id")) == id);
            if (elm == null) return;

            var status = Repository.GetStatusOrder(deliveryId);
            elm.SetElementValue("deliveryId", deliveryId);
            elm.SetElementValue("status", status);

            Extension.SaveXml(xdoc, _filename);
        }

        public static int NextId()
        {
            try
            {
                if (!File.Exists(_filename)) return 1;
                var xdoc = Extension.LoadXml(_filename);
                var root = xdoc.Element("shoppingcart");
                if (root == null || root.ToString().Equals("<shoppingcart />")) return 1;

                var last = root.Descendants("cart").Last();
                var nextId = Convert.ToInt32(last.GetElementValue("id")) + 1;
                return nextId;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Next Id: " + exception.Message);
                return -1;
            }
        }

        public static IEnumerable<Cart> GetAllShoppinCart()
        {
            //var now = DateTime.Now;
            //var date = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            if (!File.Exists(_filename)) return null;
            var xdoc = Extension.LoadXml(_filename);
            var list = from c in xdoc.Descendants("cart")
                       where Convert.ToDateTime(c.GetElementValue("date")) <= DateTime.Now
                       select new Cart
                                  {
                                      Id = Convert.ToInt32(c.GetElementValue("id")),
                                      DeliveryNoteId = Convert.ToInt32(c.GetElementValue("deliveryId")),
                                      Status = Convert.ToByte(c.GetElementValue("status")),
                                      Date = Convert.ToDateTime(c.GetElementValue("date")),
                                      TotalPrice = Convert.ToDecimal(c.GetElementValue("totalprice")),
                                      OrdersDetails = GetOrderDetails(c.Element("orderdetails")).ToList(),
                                      Customer = GetCustomer(c.Element("customer"))
                                  };
            return list;
        }

        public static IEnumerable<OrderDetail> GetOrderDetails(XElement element)
        {
            var list = from o in element.Descendants("detail")
                       select new OrderDetail
                                  {
                                      ProductId = Convert.ToInt32(o.GetElementValue("productId")),
                                      ProductName = o.GetElementValue("productName"),
                                      ProductImage = o.GetElementValue("productImage"),
                                      ProductPrice = Convert.ToDecimal(o.GetElementValue("productPrice")),
                                      CateId = Convert.ToInt16(o.GetElementValue("cateId")),
                                      CateName = o.GetElementValue("cateName"),
                                      Unit = Convert.ToInt32(o.GetElementValue("unit"))
                                  };
            return list;
        }

        public static Entity.Customer GetCustomer(XElement element)
        {
            var cust = new Entity.Customer
                           {
                               CodeId = element.GetElementValue("codeid"),
                               Name = element.GetElementValue("name"),
                               Address = element.GetElementValue("address"),
                               Phone = element.GetElementValue("phone"),
                               Email = element.GetElementValue("email")
                           };
            return cust;
        }

        public static double GetPercent()
        {
            if (!File.Exists(_fileConfig)) return 0.2;
            using (var streamReader = File.OpenText(_fileConfig))
            {
                double percent;
                return double.TryParse(streamReader.ReadLine(), out percent) ? percent : 0.2;
            }
        }
        public static void SetPercent()
        {
            try
            {
                if (File.Exists(_fileConfig))
                    File.Delete(_fileConfig);

                using (var streamWriter = new StreamWriter(new FileStream(_fileConfig, FileMode.Create)))
                {
                    streamWriter.WriteLine(Repository.XPercent);
                }

            }
            catch (Exception exception)
            {
                Debug.WriteLine("Set percent: " + exception.Message);
            }
        }
    }
}