using System;
using System.Collections.Generic;

namespace empty.Model
{
    public static class MyExtensionMethods
    {
        public static decimal TotalPrices(this IEnumerable<Product> Products)
        {
            decimal total = 0;
            foreach (Product prod in Products)
            {
                total += prod?.Price ?? 0;
            }
            return total;
        }
    }
}
