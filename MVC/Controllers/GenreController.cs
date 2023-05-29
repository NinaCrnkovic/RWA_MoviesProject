using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class GenreController : Controller
{
        private readonly ILogger<GenreController> _logger;
        private readonly IGenreRepository _genreRepo;
        private readonly IMapper _mapper;
        public GenreController(ILogger<GenreController> logger, IGenreRepository genreRepo, IMapper mapper)
        {
            _logger = logger;
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public IActionResult Genre()
        {
            var blGenres = _genreRepo.GetAll();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);
            return View(vmGenres);
        }

        public IActionResult Details(int id)
        {
            var blGenre = _genreRepo.GetById(id);
            if (blGenre == null)
            {
                return NotFound();
            }
            var vmGenre = _mapper.Map<VMGenre>(blGenre);
            return View(vmGenre);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VMGenre genre)
        {
            try
            {
                var blGenre = _mapper.Map<BLGenre>(genre);
                var newGenre = _genreRepo.Add(blGenre);
                var vmGenre = _mapper.Map<VMGenre>(newGenre);
                return RedirectToAction(nameof(Genre));
            }
            catch (Exception)
            {
                return View(genre);
            }
        }

        public IActionResult Edit(int id)
        {
            var blGenre = _genreRepo.GetById(id);
            if (blGenre == null)
            {
                return NotFound();
            }
            var vmGenre = _mapper.Map<VMGenre>(blGenre);
            return View(vmGenre);
        }

        [HttpPost]
        public IActionResult Edit(int id, VMGenre genre)
        {
            try
            {
                var blGenre = _mapper.Map<BLGenre>(genre);
                _genreRepo.Update(id, blGenre);
                return RedirectToAction(nameof(Genre));
            }
            catch (Exception)
            {
                return View(genre);
            }
        }

        public IActionResult Delete(int id)
        {
            var blGenre = _genreRepo.GetById(id);
            if (blGenre == null)
            {
                return NotFound();
            }
            var vmGenre = _mapper.Map<VMGenre>(blGenre);
            return View(vmGenre);
        }

        [HttpPost]
        public IActionResult Delete(int id, VMGenre genre)
        {
            var blGenre = _genreRepo.GetById(id);
            if (blGenre == null)
            {
                return NotFound();
            }
            _genreRepo.Delete(id);
            return RedirectToAction(nameof(Genre));
        }

    }
}
