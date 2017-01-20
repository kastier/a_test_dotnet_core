using AspCoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreTest.Repository
{
    public class BookRepository
    {
        public IEnumerable<Book> Books => new List<Book> {
            new Book() { Name = "Book1" },
            new Book() { Name = "Book2" },
            new Book() { Name = "Book3" }
        };
    }
}
