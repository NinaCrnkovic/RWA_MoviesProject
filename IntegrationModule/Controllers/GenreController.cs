using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Genre>> GetAllGenres()
        {
            try
            {
                var blGenres = _genreRepository.GetAll();
                var genres = _mapper.Map<IEnumerable<Genre>>(blGenres);
                return Ok(genres);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested" +ex);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Genre>> SearchGenre(string searchPart)
        {
            try
            {
                var dbGenres = _genreRepository.GetAll().Where(x => x.Name.Contains(searchPart));
                var genres = _mapper.Map<IEnumerable<Genre>>(dbGenres);
                return Ok(genres);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<Genre> GetGenreById(int id)
        {
            try
            {
                var blGenre = _genreRepository.GetById(id);
                if (blGenre == null)
                    return NotFound($"Could not find genre with id {id}");

                var genre = _mapper.Map<Genre>(blGenre);
                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost("[action]")]
        public ActionResult<Genre> CreateGenre(Genre genre)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                genre.Id = 0;
                var blGenre = _mapper.Map<BLGenre>(genre);
                var newGenre = _genreRepository.Add(blGenre);
                var createdGenre = _mapper.Map<Genre>(newGenre);

                return Ok(createdGenre);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the genre.");
            }
        }

        [HttpPut("[action]/{id}")]
        public ActionResult<Genre> UpdateGenre(int id, Genre genre)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

            

                var blGenre = _mapper.Map<BLGenre>(genre);
                var updatedGenre = _genreRepository.Update(id, blGenre);
                var modifiedGenre = _mapper.Map<Genre>(updatedGenre);

                return Ok(modifiedGenre);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the genre.");
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<Genre> DeleteGenre(int id)
        {
            try
            {
                var blGenre = _genreRepository.GetById(id);
                if (blGenre == null)
                    return NotFound($"Could not find genre with id {id}");
                _genreRepository.Delete(id);
                return Ok(blGenre);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the genre.");
            }
        }




    }
}
