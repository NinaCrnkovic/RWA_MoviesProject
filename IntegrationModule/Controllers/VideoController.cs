using BL.DALModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;

        public VideoController(RwaMoviesContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetAll(string name, int page = 1, int pageSize = 10, string sortBy = "id")
        {
            try
            {
                var query = _dbContext.Videos.AsQueryable();

                // Filtriranje po nazivu
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(v => v.Name.Contains(name));
                }

                // Poredak
                switch (sortBy)
                {
                    case "name":
                        query = query.OrderBy(v => v.Name);
                        break;
                    case "totaltime":
                        query = query.OrderBy(v => v.TotalSeconds);
                        break;
                    default:
                        query = query.OrderBy(v => v.Id);
                        break;
                }

                // Straničenje
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                var videos = query.ToList();

                return Ok(videos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Video> Get(int id)
        {
            try
            {
                var video = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
                if (video == null)
                    return NotFound($"Could not find video with id {id}");

                return Ok(video);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost]
        public ActionResult<Video> Post(Video video)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _dbContext.Videos.Add(video);
                _dbContext.SaveChanges();

                return Ok(video);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Video> Put(int id, Video video)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbVideo = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find video with id {id}");

                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.Image = video.Image;
                dbVideo.TotalSeconds = video.TotalSeconds;
                dbVideo.StreamingUrl = video.StreamingUrl;
                dbVideo.Genre = video.Genre;
                dbVideo.VideoTags = video.VideoTags;

                _dbContext.SaveChanges();

                return Ok(dbVideo);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Video> Delete(int id)
        {
            try
            {
                var dbVideo = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find video with id {id}");

                _dbContext.Videos.Remove(dbVideo);
                _dbContext.SaveChanges();

                return Ok(dbVideo);
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

