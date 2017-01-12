using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Person p = new Person();
            Console.WriteLine(p.Name);
            p.Name = "hi";
            Console.WriteLine(p.Name);
        }
    }
}
