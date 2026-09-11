using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs.AuthorDTO;
using LibraryAPI.IService.Interfaces;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);
            if (author == null)
                return NotFound();
            return Ok(author);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var author = await _authorService.CreateAuthor(createAuthorDTO);
            if (author == null)
                return NotFound();
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDTO updateAuthorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var answer = await _authorService.UpdateAuthor(id, updateAuthorDTO);
            if (answer == true)
                return NoContent();
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var answer = await _authorService.DeleteAuthor(id);
            if (answer == true)
                return NoContent();
            return NotFound();
        }
        [HttpHead("{id}")]
        public async Task<IActionResult> HeadAuthor(int id)
        {
            var author = await _authorService.GetAuthorById(id);
            if (author == null)
                return NotFound();
            return Ok(author);
        }
    }
}
