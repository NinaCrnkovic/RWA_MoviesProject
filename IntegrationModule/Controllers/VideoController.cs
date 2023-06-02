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
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

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

        //[HttpPost("[action]")]
        //public ActionResult<Video> CreateVideo([FromBody] Video video)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        video.Id = 0;

        //        // Mapiranje objekata
        //        var blVideo = _mapper.Map<BLVideo>(video);

        //        // Provjera i spremanje slike
        //        if (video.Image != null)
        //        {
        //            var blImage = _mapper.Map<BLImage>(video.Image);
        //            var newImage = _imageRepository.Add(blImage);
        //            blVideo.ImageId = newImage.Id;
        //        }

        //        // Spremanje videa
        //        var newVideo = _videoRepository.Add(blVideo);
        //        var createdVideo = _mapper.Map<Video>(newVideo);

        //        return Ok(createdVideo);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            "There has been a problem while creating the video.");
        //    }
        //}



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
        public ActionResult<Video> UpdateVideo(int id, Video video)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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

