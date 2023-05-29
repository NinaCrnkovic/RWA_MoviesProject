using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class TagController : Controller
{
        private readonly ILogger<TagController> _logger;
        private readonly ITagRepository _tagRepo;
        private readonly IMapper _mapper;

        public TagController(ILogger<TagController> logger, ITagRepository tagRepo, IMapper mapper)
        {
            _logger = logger;
            _tagRepo = tagRepo;
            _mapper = mapper;
        }


        public IActionResult Tag()
        {
            var blTags = _tagRepo.GetAll();
            var vmTags = _mapper.Map<IEnumerable<VMTag>>(blTags);
            return View(vmTags);
        }

        public IActionResult Details(int id)
        {
            var blTag = _tagRepo.GetById(id);
            if (blTag == null)
            {
                return NotFound();
            }
            var vmTag = _mapper.Map<VMTag>(blTag);
            return View(vmTag);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VMTag tag)
        {
            try
            {
                var blTag = _mapper.Map<BLTag>(tag);
                var newTag = _tagRepo.Add(blTag);
                var vmTag = _mapper.Map<VMTag>(newTag);
                return RedirectToAction(nameof(Tag));
            }
            catch (Exception)
            {
                return View(tag);
            }
        }

        public IActionResult Edit(int id)
        {
            var blTag = _tagRepo.GetById(id);
            if (blTag == null)
            {
                return NotFound();
            }
            var vmTag = _mapper.Map<VMTag>(blTag);
            return View(vmTag);
        }

        [HttpPost]
        public IActionResult Edit(int id, VMTag tag)
        {
            try
            {
                var blTag = _mapper.Map<BLTag>(tag);
                _tagRepo.Update(id, blTag);
                return RedirectToAction(nameof(Tag));
            }
            catch (Exception)
            {
                return View(tag);
            }
        }

        public IActionResult Delete(int id)
        {
            var blTag = _tagRepo.GetById(id);
            if (blTag == null)
            {
                return NotFound();
            }
            var vmTag = _mapper.Map<VMTag>(blTag);
            return View(vmTag);
        }

        [HttpPost]
        public IActionResult Delete(int id, VMTag tag)
        {
            var blTag = _tagRepo.GetById(id);
            if (blTag == null)
            {
                return NotFound();
            }
            _tagRepo.Delete(id);
            return RedirectToAction(nameof(Tag));
        }

    }
}
