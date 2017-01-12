using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prac
{
    public class Person
    {
        private string _name;
        public string Name
        {
            get { Console.WriteLine("汉字"); return _name+"--weiba"; }
            set { _name = value; }
        }
        public int Age { get; set; }

    }
}
