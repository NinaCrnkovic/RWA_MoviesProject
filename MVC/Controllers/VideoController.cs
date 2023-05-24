using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class VideoController : Controller
    {

        private readonly ILogger<VideoController> _logger;
        private readonly IVideoRepository _videoRepo;
        private readonly IMapper _mapper;
        public VideoController(ILogger<VideoController> logger, IVideoRepository videoRepo, IMapper mapper)
        {
            _logger = logger;
            _videoRepo = videoRepo;
            _mapper = mapper;
        }
        public IActionResult Video()
        {

            var blVideo = _videoRepo.GetAll();
            var vmVideo = _mapper.Map<IEnumerable<VMVideo>>(blVideo);
            return View(vmVideo);
        }
    }
}
