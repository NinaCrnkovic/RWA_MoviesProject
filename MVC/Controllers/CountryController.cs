using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class CountryController : Controller
{
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryRepository _countryRepo;
         private readonly IMapper _mapper;

        public CountryController(ILogger<CountryController> logger, ICountryRepository countryRepo, IMapper mapper)
        {
            _logger = logger;
            _countryRepo = countryRepo;
            _mapper = mapper;
        }


  
        public IActionResult Country()
        {

            var blCountry = _countryRepo.GetAll();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
            


            return View(vmCountry);
        }

        public IActionResult Details(int id)
        {
            var blCountry = _countryRepo.GetById(id);
            if (blCountry == null)
            {
                return NotFound();
            }
            var vmCountry = _mapper.Map<VMCountry>(blCountry);
            return View(vmCountry);
        }


        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult Create(VMCountry country)
        {
            try
            {
                var blCountry = _mapper.Map<BLCountry>(country);
                var newCountry = _countryRepo.Add(blCountry);
                var vmCountry = _mapper.Map<VMCountry>(newCountry);

                

                return RedirectToAction(nameof(Country));
            }
            catch (Exception)
            {
                

                return View(country);
            }
        }


        public IActionResult Edit(int id)
        {
            var blCountry = _countryRepo.GetById(id);
            if (blCountry == null)
            {
                return NotFound();
            }
            var vmCountry = _mapper.Map<VMCountry>(blCountry);

           

            return View(vmCountry);
        }

        [HttpPost]
        public IActionResult Edit(int id, VMCountry country)
        {
            try
            {
                var blCountry = _mapper.Map<BLCountry>(country);
                _countryRepo.Update(id, blCountry);
                return RedirectToAction(nameof(Country));
            }
            catch (Exception)
            {
                 return View(country);
            }
        }


        public IActionResult Delete(int id)
        {
            var blCountry = _countryRepo.GetById(id);
            if (blCountry == null)
            {
                return NotFound();
            }
            var vmCountry = _mapper.Map<VMCountry>(blCountry);
            return View(vmCountry);
        }

        [HttpPost]
        public IActionResult Delete(int id, VMCountry country)
        {
            var blCountry = _countryRepo.GetById(id);
            if (blCountry == null)
            {
                return NotFound();
            }
            _countryRepo.Delete(id);
            return RedirectToAction("Country");
        }

    }
}
