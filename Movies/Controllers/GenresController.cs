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
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public  IActionResult Get()
        {
            var result = _unitOfWork.Genre.GetAll(order:x=> x.Name);
            return Ok(result.OrderBy(x=> x.Name));
        }
        [HttpPost]
        public  IActionResult Post([FromBody] GenresDto dto)
        {
            var obj = new Genre
            {
                Name = dto.Name,
            };
            _unitOfWork.Genre.Create(obj);
            _unitOfWork.Save();
            return Ok(obj);
        }


        [HttpPut("{id}")]
        public  IActionResult Put(int id, [FromBody] GenresDto dto)
        {
            Genre obj = _unitOfWork.Genre.GetSingleOrDefault(x=>x.Id == id);
            if(obj == null)
            {
                return NotFound($"no genre was found with Id {id}");
            }
            obj.Name = dto.Name;
            _unitOfWork.Genre.Update(obj);
            _unitOfWork.Save();
            return Ok(obj);
        }

        [HttpDelete("{id}")]
        public  IActionResult Delete(int id)
        {
            Genre obj = _unitOfWork.Genre.GetSingleOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound($"no genre was found with Id {id}");
            }
            _unitOfWork.Genre.Delete(obj);
            _unitOfWork.Save();
            return Ok(obj);
        }
    }
}
