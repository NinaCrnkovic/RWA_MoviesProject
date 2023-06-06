using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PublicModule.ViewModels;

namespace MVC_PublicModule.Controllers
{

    public class VideoController : Controller
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoRepository _videoRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IImageRepository _imageRepo;

        private readonly IMapper _mapper;
        public VideoController(ILogger<VideoController> logger, IVideoRepository videoRepo, IGenreRepository genreRepo, IImageRepository imageRepo, IMapper mapper)
        {
            _logger = logger;
            _videoRepo = videoRepo;
            _mapper = mapper;
            _genreRepo = genreRepo;
            _imageRepo = imageRepo;
        }



        public IActionResult Video(int page, int size, string orderBy, string direction, string videoName)
        {
            // Set up some default values
            if (size == 0)
                size = 3;

            var blVideo = _videoRepo.GetPagedData(page, size, orderBy, direction);
            var vmVideo = _mapper.Map<IEnumerable<VMVideo>>(blVideo);

            foreach (var video in vmVideo)
            {
                var blGenre = _genreRepo.GetById(video.GenreId);
                video.GenreName = blGenre?.Name;

                var blImage = _imageRepo.GetById(video.ImageId);
                video.ImageContent = blImage?.Content;
            }

            

            ViewData["page"] = page;
            ViewData["size"] = size;
            ViewData["orderBy"] = orderBy;
            ViewData["direction"] = direction;
            ViewData["totalPages"] = _videoRepo.GetTotalCount() / size;
            return View(vmVideo);
        }


        public IActionResult VideoTableBodyPartial(int page, int size, string orderBy, string direction, string videoName)
        {
            // Set up some default values
            if (size == 0)
                size = 3;

            var blVideo = _videoRepo.GetPagedData(page, size, orderBy, direction);
            var vmVideo = _mapper.Map<IEnumerable<VMVideo>>(blVideo);

            foreach (var video in vmVideo)
            {
                var blGenre = _genreRepo.GetById(video.GenreId);
                video.GenreName = blGenre?.Name;

                var blImage = _imageRepo.GetById(video.ImageId);
                video.ImageContent = blImage?.Content; // Pridružite URL slike
            }

            ViewData["page"] = page;
            ViewData["size"] = size;
            ViewData["orderBy"] = orderBy;
            ViewData["direction"] = direction;
            ViewData["totalPages"] = _videoRepo.GetTotalCount() / size;
      

            return PartialView("VideoTableBodyPartial", vmVideo);
        }

        public IActionResult GetVideoData(string term)
        {
            var filteredGenres = _videoRepo.GetFilteredData(term);
            var labeledValues = filteredGenres.Select(x => new { value = x.Id, label = x.Name });

            return Json(labeledValues);
        }

        public IActionResult FilterVideos(string videoName)
        {
            var blVideos = _videoRepo.GetFilteredData(videoName);
            var vmVideos = _mapper.Map<IEnumerable<VMVideo>>(blVideos);

            foreach (var video in vmVideos)
            {
                var blGenre = _genreRepo.GetById(video.GenreId);
                video.GenreName = blGenre?.Name;

                var blImage = _imageRepo.GetById(video.ImageId);
                video.ImageContent = blImage?.Content;
            }

            ViewData["page"] = 0; 
            if (ViewData.ContainsKey("size"))
            {
                ViewData["size"] = (int)ViewData["size"]; // Sačuvajte trenutnu veličinu stranice
            }
            ViewData["orderBy"] = (string)ViewData["orderBy"]; // Sačuvajte trenutni redosled sortiranja
            ViewData["direction"] = (string)ViewData["direction"]; // Sačuvajte trenutni smer sortiranja
            if (ViewData.ContainsKey("size"))
            {
                int size = (int)ViewData["size"];
                ViewData["totalPages"] = _videoRepo.GetTotalCount() / size;
            }


            return PartialView("VideoTableBodyPartial", vmVideos);
        }



        public IActionResult Details(int id)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }

            var vmVideo = _mapper.Map<VMVideo>(blVideo);

            var genre = _genreRepo.GetById(vmVideo.GenreId);
            var image = _imageRepo.GetById(vmVideo.ImageId);

            ViewBag.GenreName = genre?.Name;
            ViewBag.ImageName = image?.Content;

            var videoTags = _videoRepo.GetVideoTagsByVideoId(id);
            var vmVideoTags = videoTags.ToList();

            ViewBag.VideoTags = vmVideoTags;

            return View(vmVideo);
        }
    }
}
