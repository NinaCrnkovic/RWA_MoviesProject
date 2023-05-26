using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
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
                vmVideo = vmVideo.Where(v => v.Name.Contains(videoName));
            }

            // Filtriranje prema nazivu žanra
            if (!string.IsNullOrEmpty(genreName))
            {
                vmVideo = vmVideo.Where(v => v.GenreName.Contains(genreName));
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

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(VMVideo video)
        {
            try
            {
                var blVideo = _mapper.Map<BLVideo>(video);
                var newVideo = _videoRepo.Add(blVideo);
                var vmVideo = _mapper.Map<VMVideo>(newVideo);
                return RedirectToAction(nameof(Video));
            }
            catch (Exception)
            {

                return View(video);
            }
           
                         
        }

       
       

        public IActionResult Edit(int id)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }
            var vmVideo = _mapper.Map<VMVideo>(blVideo);
            return View(vmVideo);
        }

        [HttpPost]
        public IActionResult Edit(int id, VMVideo video)
        {
           try
            {
                var blVideo = _mapper.Map<BLVideo>(video);
           
                _videoRepo.Update(id, blVideo);
                return RedirectToAction("Video");
            }
            catch(Exception)
            {
               
                return View(video);
            }
            
        }

        public IActionResult Delete(int id)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }
            var vmVideo = _mapper.Map<VMVideo>(blVideo);
            return View(vmVideo);
        }

        [HttpPost]
        public IActionResult Delete(int id, VMVideo video)
        {
            var blVideo = _videoRepo.GetById(id);
            if (blVideo == null)
            {
                return NotFound();
            }
            _videoRepo.Delete(id);
            return RedirectToAction("Video");
        }

    }
}
