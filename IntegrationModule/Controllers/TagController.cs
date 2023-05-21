using BL.DALModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;

        public TagController(RwaMoviesContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Tag>> GetAll()
        {
            try
            {
                return _dbContext.Tags;
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Tag>> Search(string searchPart)
        {
            try
            {
                var dbGenres = _dbContext.Tags.Where(x => x.Name.Contains(searchPart));
                return Ok(dbGenres);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Tag> Get(int id)
        {
            try
            {
                var dbGenre = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbGenre == null)
                    return NotFound($"Could not find genre with id {id}");

                return Ok(dbGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost()]
        public ActionResult<Genre> Post(Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _dbContext.Tags.Add(tag);

                _dbContext.SaveChanges();

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Tag> Put(int id, Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbTag = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbTag == null)
                    return NotFound($"Could not find genre with id {id}");

                dbTag.Name = tag.Name;

                _dbContext.SaveChanges();

                return Ok(dbTag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Genre> Delete(int id)
        {
            try
            {
                var dbTag = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbTag == null)
                    return NotFound($"Could not find genre with id {id}");

                _dbContext.Tags.Remove(dbTag);

                _dbContext.SaveChanges();

                return Ok(dbTag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

    }
}

