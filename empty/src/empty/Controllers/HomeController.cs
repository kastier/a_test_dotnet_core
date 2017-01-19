using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using empty.Model;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace empty.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public ViewResult Index()
        {
            empty.Model.ShoppingCart cart = new Model.ShoppingCart { Products = empty.Model.Product.GetProducts() };
            empty.Model.Product[] productArray =
             {
                new Model.Product {Name="Kayak",Price=23134},
                new Model.Product {Name="LifeJacket",Price=2124},
            };
            decimal cartTotal = cart.TotalPrices();
            decimal arrayTotal = productArray.TotalPrices(); 
            return View("Index", new String[] {
                $"cart total:{cartTotal:C2}",
                $"Array Total:{cartTotal:C2}"});
            
        }
    }
}
