using AutoMapper;
using BL.Repositories;
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

            var blVideo = _videoRepo.GetPagedData(page, size, orderBy, direction, videoName?.ToLower(), null);
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
            ViewData["totalPages"] = (int)Math.Ceiling((double)_videoRepo.GetTotalCount() / size);

            return View(vmVideo);
        }

        public IActionResult VideoTableBodyPartial(int page, int size, string orderBy, string direction, string videoName)
        {
            // Set up some default values
            if (size == 0)
                size = 3;

            var blVideo = _videoRepo.GetPagedData(page, size, orderBy, direction, videoName, null);
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
            ViewData["totalPages"] = (int)Math.Ceiling((double)_videoRepo.GetTotalCount() / size);

            return PartialView("VideoTableBodyPartial", vmVideo);
        }







        public IActionResult Details(int id)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }
            var vmVideo = _mapper.Map<VMVideo>(blVideo);

            // Dohvatite naziv žanra i naziv slike
            var genre = _genreRepo.GetById(vmVideo.GenreId);
            var image = _imageRepo.GetById(vmVideo.ImageId);

            // Pohranite nazive u ViewBag
            ViewBag.GenreName = genre?.Name;
            ViewBag.ImageName = image?.Content;

            return View(vmVideo);
        }
    }
}
