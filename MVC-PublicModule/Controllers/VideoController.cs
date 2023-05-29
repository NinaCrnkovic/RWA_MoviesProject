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
        public IActionResult Video(string videoName, string genreName)
        {

            var blVideo = _videoRepo.GetAll();
            var vmVideo = _mapper.Map<IEnumerable<VMVideo>>(blVideo);
            foreach (var video in vmVideo)
            {
                var blGenre = _genreRepo.GetById(video.GenreId);
                video.GenreName = blGenre.Name;
                var blImage = _imageRepo.GetById(video.ImageId);
                video.ImageContent = blImage.Content;

            }
            // Filtriranje prema nazivu videosadržaja
            if (!string.IsNullOrEmpty(videoName))
            {
                vmVideo = vmVideo.Where(v => v.Name.IndexOf(videoName, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Filtriranje prema nazivu žanra
            if (!string.IsNullOrEmpty(genreName))
            {
                vmVideo = vmVideo.Where(v => v.GenreName.IndexOf(genreName, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Dodaje filtere u URL
            ViewData["VideoName"] = videoName;
            ViewData["GenreName"] = genreName;

            // Dodavanje filtera u kolačiće

            if (!string.IsNullOrEmpty(videoName))
            {
                Response.Cookies.Append("VideoName", videoName);
            }

            if (!string.IsNullOrEmpty(genreName))
            {
                Response.Cookies.Append("GenreName", genreName);
            }


            return View(vmVideo);
        }

        public IActionResult Details(int id)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }
            var vmVideo = _mapper.Map<VMVideo>(blVideo);
            return View(vmVideo);
        }
    }
}
