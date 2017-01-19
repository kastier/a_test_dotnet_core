using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty.Model
{
    public class Product
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public static Product[] GetProducts() {
            Product kayak = new Product
            {
                Name = "Kayak",
                Price = 124M

            };
            Product lifejacket = new
            Product
            {
                Name = "LifeJacket",
                Price = 49.95M
            };
            return new Product[] { kayak, lifejacket, null };
        } 
    }
}
