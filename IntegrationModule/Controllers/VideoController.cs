using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public VideoController(IVideoRepository videoRepository, IImageRepository imageRepository, IGenreRepository genreRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _imageRepository = imageRepository;
            _tagRepository = tagRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }


        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Video>> GetAll(string name, int page = 1, int pageSize = 10, string sortBy = "id")
        {
            try
            {
                var blVideo = _videoRepository.GetAll();
                var videos = _mapper.Map<IEnumerable<Video>>(blVideo);
        
                var query = videos.AsQueryable();
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
                query = query.Skip((page) * pageSize).Take(pageSize);

                videos = query.ToList();

                return Ok(videos);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Video>> SearchVideo(string searchPart)
        {
            try
            {
                var dbVideo = _videoRepository.GetAll().Where(x => x.Name.ToLower().Contains(searchPart.ToLower()));
                var videos = _mapper.Map<IEnumerable<Video>>(dbVideo);
                return Ok(videos);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }


        [HttpGet("[action]/{id}")]
        public ActionResult<Video> GetVideoById(int id)
        {
            try
            {
                var blVideo = _videoRepository.GetById(id);
                if (blVideo == null)
                    return NotFound($"Could not find genre with id {id}");

                var video = _mapper.Map<Video>(blVideo);
                return Ok(video);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }





        [HttpPost("[action]")]
        public ActionResult<Video> CreateVideo([FromQuery] string name, [FromQuery] string description, [FromQuery] int genreId, [FromQuery] int totalSeconds, [FromQuery] string streamingUrl, [FromQuery] int imageId, [FromQuery] List<int> tagIds)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var video = new Video
                {
                    Id = 0,
                    CreatedAt = DateTime.Now,
                    Name = name,
                    Description = description,
                    GenreId = genreId,
                    TotalSeconds = totalSeconds,
                    StreamingUrl = streamingUrl,
                    ImageId = imageId,
                    Genre = null,
                    Image = null,
                    VideoTags = new List<VideoTag>()
                };

                // Provjera i spremanje oznaka
                if (tagIds != null && tagIds.Any())
                {
                    foreach (var tagId in tagIds)
                    {
                        var videoTag = new VideoTag
                        {
                            VideoId = video.Id,
                            TagId = tagId
                        };

                        video.VideoTags.Add(videoTag);
                    }
                }

                // Spremanje videa
                var blVideo = _mapper.Map<BLVideo>(video);
                var newVideo = _videoRepository.Add(blVideo);
                var createdVideo = _mapper.Map<Video>(newVideo);

                return Ok(createdVideo);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while creating the video.");
            }
        }




        [HttpPut("[action]/{id}")]
        public ActionResult<Video> UpdateVideo(int id, [FromQuery] string name, [FromQuery] string description, [FromQuery] int genreId, [FromQuery] int totalSeconds, [FromQuery] string streamingUrl, [FromQuery] int imageId, [FromQuery] List<int> tagIds)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var video = _videoRepository.GetById(id);
                if (video == null)
                    return NotFound();

                video.Name = name;
                video.Description = description;
                video.GenreId = genreId;
                video.TotalSeconds = totalSeconds;
                video.StreamingUrl = streamingUrl;
                video.ImageId = imageId;
                video.VideoTags = new List<BLVideoTag>();

                // Provera i dodavanje tagova
                if (tagIds != null && tagIds.Any())
                {
                    foreach (var tagId in tagIds)
                    {
                        var videoTag = new BLVideoTag
                        {
                            VideoId = video.Id,
                            TagId = tagId
                        };

                        video.VideoTags.Add(videoTag);
                    }
                }

                // Ažuriranje videa
                var blVideo = _mapper.Map<BLVideo>(video);
                var updatedVideo = _videoRepository.Update(id, blVideo);
                var modifiedVideo = _mapper.Map<Video>(updatedVideo);

                return Ok(modifiedVideo);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested" + ex);
            }
        }


        [HttpDelete("[action]/{id}")]
        public ActionResult<Video> DeleteVideo(int id)
        {
            try
            {
                var blVideo = _videoRepository.GetById(id);
                if (blVideo == null)
                    return NotFound($"Could not find genre with id {id}");
                _videoRepository.Delete(id);
          

                return Ok(blVideo);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }
    }
}

