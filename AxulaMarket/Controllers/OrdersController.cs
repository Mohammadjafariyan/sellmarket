using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using sellmarket.Models;

namespace sellmarket.Controllers
{
    public class OrdersController:Controller
    {
        public ActionResult Create(bool isSample,string productName)
        {
            ViewBag.isSample = isSample;
            ViewBag.productName = productName;
            
            
            /*using (var db = new MarketContext())
            {
                var products= db.Products.ToList();
              ViewBag.products=  new SelectList(products,"Name","Id");
            }*/
            return View();
        }
        
        [System.Web.Mvc.HttpPost]
        public ActionResult Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)
                    HttpStatusCode.InternalServerError, "مقادیر ارسالی اشتباه");
            }

            using (var db = new MarketContext())
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }
            
            return RedirectToAction("Success",new {id=order.Id});
        }
        
        public ActionResult Success(int id)
        {
            ViewBag.id = id;
            
            
            return View();
        }
        
        
    }
}