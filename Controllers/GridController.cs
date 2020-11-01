using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.JavaScript.DataSources;
using System.Collections;
using Syncfusion.JavaScript;
using Syncfusion.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SyncfusionMvcApplication3;
using SyncfusionMvcApplication3.Grid;
namespace SyncfusionMvcApplication3.Controllers
{
    public class GridController : Controller
    {
        //
        // GET: /Grid/
        public ActionResult GridFeatures()
        {
            var DataSource = OrderRepository.GetAllRecords();
            ViewBag.datasource = DataSource;
            return View();
        }
    }
    public class OrderRepository
    {
        public static IList<EditableOrder> GetAllRecords()
        {
            IList<EditableOrder> orders = (IList<EditableOrder>)HttpContext.Current.Session["Orders"];
            if (orders == null)
            {
                HttpContext.Current.Session["Orders"] = orders = (from ord in new NorthwindDataContext().Orders.Take(200)
                                                                  select new EditableOrder
                                                                  {
                                                                      OrderID = ord.OrderID,
                                                                      OrderDate = ord.OrderDate,
                                                                      CustomerID = ord.CustomerID,
                                                                      EmployeeID = ord.EmployeeID,
                                                                      Freight = ord.Freight,
                                                                      ShipAddress = ord.ShipAddress,
                                                                      ShipCity = ord.ShipCity,
                                                                      ShipName = ord.ShipName,
                                                                      ShipPostalCode = ord.ShipPostalCode,
                                                                      ShipRegion = ord.ShipRegion,
                                                                      ShipCountry = ord.ShipCountry
                                                                  }).ToList();
                foreach (var order in orders)
                {
                    if (order.Freight > 30)
                        order.Verified = true;
                    else
                        order.Verified = false;
                    if (order.Freight > 50)
                        order.Verified2 = true;
                    else
                        order.Verified2 = true;
                }
            }
            return orders;
        }
        public static void Add(EditableOrder order)
        {
            int id = GetAllRecords().Max(o => o.OrderID);
            order.OrderID = id + 1;
            GetAllRecords().Insert(0, order);
        }
        public static void Add(List<EditableOrder> order)
        {
            foreach (var temp in order)
                GetAllRecords().Insert(0, temp);
        }
        public static void Delete(int OrderID)
        {
            EditableOrder result = GetAllRecords().Where(o => o.OrderID == OrderID).FirstOrDefault();
            GetAllRecords().Remove(result);
        }
        public static void Delete(List<EditableOrder> order)
        {
            foreach (var temp in order)
            {
                EditableOrder result = GetAllRecords().Where(o => o.OrderID == temp.OrderID).FirstOrDefault();
                GetAllRecords().Remove(result);
            }
        }
        public static void Update(EditableOrder order)
        {
            EditableOrder result = GetAllRecords().Where(o => o.OrderID == order.OrderID).FirstOrDefault();
            if (result != null)
            {
                result.OrderID = order.OrderID;
                result.OrderDate = order.OrderDate;
                result.CustomerID = order.CustomerID;
                result.EmployeeID = order.EmployeeID;
                result.Freight = order.Freight;
                result.ShipAddress = order.ShipAddress;
                result.ShipCity = order.ShipCity;               
                result.ShipName = order.ShipName;                
                result.ShipPostalCode = order.ShipPostalCode;
                result.ShipRegion = order.ShipRegion;
                result.ShipCountry = order.ShipCountry;
                result.Verified = order.Verified;
            }
        }              
        public static void Update(List<EditableOrder> order)
        {
            foreach (var temp in order)
            {
                EditableOrder result = GetAllRecords().Where(o => o.OrderID == temp.OrderID).FirstOrDefault();
                if (result != null)
                {
                    result.OrderID = temp.OrderID;
                    result.OrderDate = temp.OrderDate;
                    result.CustomerID = temp.CustomerID;
                    result.EmployeeID = temp.EmployeeID;
                    result.Freight = temp.Freight;
                    result.ShipAddress = temp.ShipAddress;
                    result.ShipCity = temp.ShipCity;
                    result.ShipName = temp.ShipName;
                    result.ShipPostalCode = temp.ShipPostalCode;
                    result.ShipRegion = temp.ShipRegion;
                    result.ShipCountry = temp.ShipCountry;
                    result.Verified = temp.Verified;
                }
            }
        }
    }
	public class EditableOrder
    {
        [Range(0, int.MaxValue, ErrorMessage = "OrderID must be greater than 0.")]
        public int OrderID
        {
            get;
            set;
        }
        [StringLength(5, ErrorMessage = "CustomerID must be 5 characters.")]
        public string CustomerID
        {
            get;
            set;
        }
        [Range(1, 9, ErrorMessage = "EmployeeID must be between 0 and 9.")]
        public int? EmployeeID
        {
            get;
            set;
        }
        [RegularExpression(@"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$", ErrorMessage = "Date is required")]
        public DateTime? OrderDate
        {
            get;
            set;
        }
        public string ShipName
        {
            get;
            set;
        }
        [StringLength(15, ErrorMessage = "ShipCity must be 15 characters.")]
        public string ShipCity
        {
            get;
            set;
        }
        public string ShipAddress
        {
            get;
            set;
        }
        public string ShipRegion
        {
            get;
            set;
        }
        public string ShipPostalCode
        {
            get;
            set;
        }
        [StringLength(15, ErrorMessage = "ShipName must be 15 characters.")]
        public string ShipCountry
        {
            get;
            set;
        }
        [Range(1.00, 1000.00, ErrorMessage = "Freight must be between 1.00 & 1000")]
        public decimal? Freight
        {
            get;
            set;
        }
        public bool Verified
        {
            get;
            set;
        }
        public bool Verified2
        {
            get;
            set;
        }
    }
}
