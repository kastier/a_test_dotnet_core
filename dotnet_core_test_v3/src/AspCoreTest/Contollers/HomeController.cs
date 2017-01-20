using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.Contollers
{
    public class HomeController:Controller
    {

        public ViewResult Index()
        {
            //throw new Exception("哈哈");
            return View(new[] {"C#", "Language", "Features"});
        }

        public ViewResult LoadTest()
        {
            return View();
        }

        public ViewResult LoadTestStatic()
        {
            return View();
        }
    }
}
