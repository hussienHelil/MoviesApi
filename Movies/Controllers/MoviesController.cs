using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Dtos;
using Movies.Model;
using Movies.Repository.IRepository;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly List<string> _alloewdExtensions = new List<string> {".jpg", ".png"};
        private readonly double _maxSize = 5 * 1024 * 1024;//5MB

        public MoviesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movies = _unitOfWork.Movies.GetAll(IncludedNavigations:"Genre",order:x=>x.Title);
            return Ok(movies);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = _unitOfWork.Movies.GetSingleOrDefault(x => x.Id == id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        //Get Movies with specific genre id
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreId(byte genreId)
        {
            var movies = _unitOfWork.Movies.GetAll("Genre",x=> x.GenreId == genreId,x=> x.Title);
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]MovieDto dto)
        {
            if(dto.Poster == null)
                return BadRequest("Poster Is Required");


            if (!_alloewdExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only Jpg and Png Extension allowed");

            if(dto.Poster.Length > _maxSize)
                return BadRequest("Maximum size of file is 5MB");

            if(_unitOfWork.Genre.GetSingleOrDefault(x=> x.Id == dto.GenreId) == null)
                return BadRequest("Invalid GenreId");

            using var memoryStream = new MemoryStream();
            dto.Poster.CopyTo(memoryStream);

            Movie movie = new Movie
            {
                Year = dto.Year,
                Title = dto.Title, 
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                GenreId = dto.GenreId,
                Poster = memoryStream.ToArray(),
            };
            _unitOfWork.Movies.Create(movie);
            _unitOfWork.Save();

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] MovieDto dto)
        {
            Movie obj = _unitOfWork.Movies.GetSingleOrDefault(x => x.Id == id);
            if (obj == null)
                return NotFound($"no genre was found with Id {id}");
            if (_unitOfWork.Genre.GetSingleOrDefault(x => x.Id == dto.GenreId) == null)
                return BadRequest("Invalid GenreId");

            if(dto.Poster != null)
            {
                if (!_alloewdExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only Jpg and Png Extension allowed");

                if (dto.Poster.Length > _maxSize)
                    return BadRequest("Maximum size of file is 5MB");

                using var mrmoryStream = new MemoryStream();
                dto.Poster.CopyTo(mrmoryStream);
                obj.Poster = mrmoryStream.ToArray();
            }


            obj.Title = dto.Title;
            obj.Storeline = dto.Storeline;
            obj.Year = dto.Year;
            obj.Rate = dto.Rate;
            obj.GenreId = dto.GenreId;
            _unitOfWork.Movies.Update(obj);
            _unitOfWork.Save();
            return Ok(obj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Movie obj =  _unitOfWork.Movies.GetSingleOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound($"no movie was found with Id {id}");
            }
            _unitOfWork.Movies.Delete(obj);
            _unitOfWork.Save();
            return Ok(obj);
        }
    }
}
