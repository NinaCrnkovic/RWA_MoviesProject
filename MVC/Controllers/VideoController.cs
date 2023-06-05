using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class VideoController : Controller
    {

        private readonly ILogger<VideoController> _logger;
        private readonly IVideoRepository _videoRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IImageRepository _imageRepo;
        private readonly ITagRepository _tagRepo;

        private readonly IMapper _mapper;
        public VideoController(ILogger<VideoController> logger, IVideoRepository videoRepo, IGenreRepository genreRepo, IImageRepository imageRepo, ITagRepository tagRepository, IMapper mapper)
        {
            _logger = logger;
            _videoRepo = videoRepo;
            _mapper = mapper;
            _genreRepo = genreRepo;
            _imageRepo = imageRepo;
            _tagRepo  = tagRepository;
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



        public IActionResult GetVideo(string video, string genre)
        {
            var filteredGenres = _videoRepo.GetFilteredDataNameAndGenre(video, genre);
            var labeledValues = filteredGenres.Select(x => new { value = x.Id, label = x.Name });

            return Json(labeledValues);
        }


        

        public IActionResult FilterVideos(string videoName, string videoGenre)
        {
            var blVideos = _videoRepo.GetFilteredDataNameAndGenre(videoName, videoGenre);
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

            // Dohvatite naziv žanra i naziv slike
            var genre = _genreRepo.GetById(vmVideo.GenreId);
            var image = _imageRepo.GetById(vmVideo.ImageId);

            // Pohranite nazive u ViewBag
            ViewBag.GenreName = genre?.Name;
            ViewBag.ImageName = image?.Content;

            return View(vmVideo);
        }


        public IActionResult Create()
        {
            var blGenres = _genreRepo.GetAll();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
            ViewBag.Genres = new SelectList(vmGenres, "Id", "Name");

            var blImages = _imageRepo.GetAll();
            var vmImages = _mapper.Map<IEnumerable<VMImage>>(blImages);
            ViewBag.Images = new SelectList(vmImages, "Id", "Content");

            var blTags = _tagRepo.GetAll();
            var vmTags = _mapper.Map<IEnumerable<VMTag>>(blTags);
            ViewBag.Tags = new SelectList(vmTags, "Id", "Name");

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

                var blGenres = _genreRepo.GetAll();
                var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
                ViewBag.Genres = new SelectList(vmGenres, "Id", "Name");

                var blTags = _tagRepo.GetAll();
                var vmTags = _mapper.Map<IEnumerable<VMTag>>(blTags);
                ViewBag.Tags = new SelectList(vmTags, "Id", "Name");

                var blImages = _imageRepo.GetAll();
                var vmImages = _mapper.Map<IEnumerable<VMImage>>(blImages);
                ViewBag.Images = new SelectList(vmImages, "Id", "Content");

                return RedirectToAction(nameof(Video));
            }
            catch (Exception)
            {
                var blGenres = _genreRepo.GetAll();
                var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
                ViewBag.Genres = new SelectList(vmGenres, "Id", "Name");

                var blTags = _tagRepo.GetAll();
                var vmTags = _mapper.Map<IEnumerable<VMTag>>(blTags);
                ViewBag.Tags = new SelectList(vmTags, "Id", "Name");

                var blImages = _imageRepo.GetAll();
                var vmImages = _mapper.Map<IEnumerable<VMImage>>(blImages);
                ViewBag.Images = new SelectList(vmImages, "Id", "Content");

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

            var blGenres = _genreRepo.GetAll();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
            ViewBag.Genres = new SelectList(vmGenres, "Id", "Name");

            var blImages = _imageRepo.GetAll();
            var vmImages = _mapper.Map<IEnumerable<VMImage>>(blImages);
            ViewBag.Images = new SelectList(vmImages, "Id", "Content");

            return View(vmVideo);
        }

        [HttpPost]
        public IActionResult Edit(int id, VMVideo video)
        {
            try
            {
                var blVideo = _mapper.Map<BLVideo>(video);
                _videoRepo.Update(id, blVideo);
                return RedirectToAction(nameof(Video));
            }
            catch (Exception)
            {
                var blGenres = _genreRepo.GetAll();
                var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
                ViewBag.Genres = new SelectList(vmGenres, "Id", "Name");

                var blImages = _imageRepo.GetAll();
                var vmImages = _mapper.Map<IEnumerable<VMImage>>(blImages);
                ViewBag.Images = new SelectList(vmImages, "Id", "Content");

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
