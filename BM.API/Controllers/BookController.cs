using BM.Core.Model;
using BM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BM.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _services;
        public BookController(IServiceManager services)
        {
            this._services = services ?? throw new ArgumentNullException(nameof(services));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                //get all books
                return Ok(await _services.BookServices.GetAllBooks());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{id}", Name = "GetBookById")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                return Ok(await _services.BookServices.GetBookById(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book request)
        {
            try
            {
                if (request == null) return BadRequest();
                await _services.BookServices.AddBook(request!);

                return Ok(request);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
