using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspCoreTest.Models;
using AspCoreTest.Repository;

namespace AspCoreTest.Contollers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private BookRepository _bookRepository;

        public BookController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public List<Book> Get()
        {
            return _bookRepository.Books.ToList();
        }

        [HttpGet("{id}")]
        public Book Get(int id)
        {
            return _bookRepository.Books.Skip(id - 1).FirstOrDefault();
        }
    }
}
