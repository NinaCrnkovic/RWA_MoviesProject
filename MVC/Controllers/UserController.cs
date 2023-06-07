using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, IUserRepository userRepo,ICountryRepository countryRepo, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;  
            _mapper = mapper;
            _countryRepo = countryRepo;
        }
        public IActionResult Index(string firstName, string lastName, string username, int countryId)
        {
            var blUsers = _userRepo.GetAll();

            // Dohvat kolačića za filtriranje
            if (!string.IsNullOrEmpty(Request.Cookies["firstName"]))
            {
                firstName = Request.Cookies["firstName"];
            }

            if (!string.IsNullOrEmpty(Request.Cookies["lastName"]))
            {
                lastName = Request.Cookies["lastName"];
            }

            if (!string.IsNullOrEmpty(Request.Cookies["username"]))
            {
                username = Request.Cookies["username"];
            }

            if (!string.IsNullOrEmpty(Request.Cookies["countryId"]))
            {
                int.TryParse(Request.Cookies["countryId"], out countryId);
            }

            // Primijenite filtre na listu korisnika
            if (!string.IsNullOrEmpty(firstName))
            {
                blUsers = blUsers.Where(u => u.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                blUsers = blUsers.Where(u => u.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(username))
            {
                blUsers = blUsers.Where(u => u.Username.Contains(username));
            }

            if (countryId > 0)
            {
                blUsers = blUsers.Where(u => u.CountryOfResidenceId == countryId);
            }

            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);

            // Dohvatite imena država za prikaz u pogledu
            var countryIds = vmUsers.Select(u => u.CountryOfResidenceId).Distinct();
            var countryNames = _countryRepo.GetAll()
                .Where(c => countryIds.Contains(c.Id))
                .ToDictionary(c => c.Id, c => c.Name);

            // Ažurirajte imena država u modelu korisnika
            foreach (var user in vmUsers)
            {
                if (countryNames.TryGetValue(user.CountryOfResidenceId, out string countryName))
                {
                    if (user.CountryOfResidence == null)
                    {
                        user.CountryOfResidence = new VMCountry();
                    }

                    user.CountryOfResidence.Name = countryName;
                }
            }


            var blCountry = _countryRepo.GetAll();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
            ViewBag.Country = new SelectList(vmCountry, "Id", "Name");

            return View(vmUsers);
        }




        public IActionResult Details(int id)
        {
            var blUser = _userRepo.GetById(id);
            if (blUser == null)
            {
                return NotFound();
            }
            var vmUser = _mapper.Map<VMUser>(blUser);

            var country = _countryRepo.GetById(vmUser.CountryOfResidenceId);

            ViewBag.CountryName = country?.Name;

            return View(vmUser);
        }

        public IActionResult Create()
        {
            var blCountry = _countryRepo.GetAll();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
            ViewBag.Country = new SelectList(vmCountry, "Id", "Name");

            return View();


        }


        [HttpPost]
        public IActionResult Create(VMRegister register)
        {
            if (!ModelState.IsValid)
                return View(register);

            var user = _userRepo.CreateUserMVC(
                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Phone,
                register.Password,
                register.CountryOfResidenceId);

            return RedirectToAction("Index");

        }


        public IActionResult Edit(int id)
        {
            var blUser = _userRepo.GetById(id);
            if (blUser == null)
            {
                return NotFound();
            }
            var vmUser = _mapper.Map<VMUser>(blUser);

            var blCountry = _countryRepo.GetAll();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
            ViewBag.Country = new SelectList(vmCountry, "Id", "Name", vmUser.CountryOfResidenceId);

            return View(vmUser);
        }

        [HttpPost]
        public IActionResult Edit(VMUser user)
        {
            if (!ModelState.IsValid)
            {
                var blCountry = _countryRepo.GetAll();
                var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
                ViewBag.Country = new SelectList(vmCountry, "Id", "Name", user.CountryOfResidenceId);

                return View(user);
            }

            var blUser = _mapper.Map<BLUser>(user);
            _userRepo.UpdateUser(blUser.Id, blUser);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var blUser = _userRepo.GetById(id);
            if (blUser == null)
            {
                return NotFound();
            }
            var vmUser = _mapper.Map<VMUser>(blUser);

            return View(vmUser);
        }




        [HttpPost]
        public IActionResult Delete(int id, VMUser user)
        {
            var blUser = _userRepo.GetById(id);
            if (blUser == null)
            {
                return NotFound();
            }

     
            _userRepo.SoftDeleteUser(blUser.Id);


            return RedirectToAction("Index");
        }


    }
}

  
