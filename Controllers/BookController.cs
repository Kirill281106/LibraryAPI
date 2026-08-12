using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using LibraryAPI.DTOs;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.IService.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewBook(CreateBookDTO crateBookDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var book = await _bookService.CreateBook(crateBookDTO);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDTO crateBookDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var ansver = await _bookService.UpdateBook(id, crateBookDTO);
            if(ansver==true)
                return NoContent();
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var ansver = await _bookService.DeleteBook(id);
            if (ansver == true)
                return NoContent();
            return NotFound();
        }
    }
}
