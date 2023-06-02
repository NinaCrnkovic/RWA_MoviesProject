using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_PublicModule.ViewModels;
using System.Security.Claims;

namespace MVC_PublicModule.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
    

        public UserController(ILogger<UserController> logger, IUserRepository userRepo, ICountryRepository countryRepo, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;
            _mapper = mapper;
            _countryRepo = countryRepo;
           
        }
        public IActionResult Home()
        {


            return View();
        }


        public IActionResult Index()
        {
            var blUsers = _userRepo.GetAll();
            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);

            return View(vmUsers);
        }

        public IActionResult Details(string username)
        {
            var blUser = _userRepo.GetByUsername(username);
            if (blUser == null)
            {
                return NotFound();
            }
            var vmUser = _mapper.Map<VMUser>(blUser);

            var country = _countryRepo.GetById(vmUser.CountryOfResidenceId);

            ViewBag.CountryName = country?.Name;

            return View(vmUser);
        }


        public IActionResult Register()
        {
            var blCountry = _countryRepo.GetAll();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
            ViewBag.Country = new SelectList(vmCountry, "Id", "Name");
            return View();
        }

        [HttpPost]
    
        public IActionResult Register(VMRegister register)
        {
            if (!ModelState.IsValid)
            {
                var blCountry = _countryRepo.GetAll();
                var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);
                ViewBag.Country = new SelectList(vmCountry, "Id", "Name");
                return View(register);
            }

            var user = _userRepo.CreateUserMVC(

                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Phone,
                register.Password,
                register.CountryOfResidenceId);
       


            return RedirectToAction("Login");
        }

        public IActionResult ValidateEmail(VMValidateEmail validateEmail)
        {
            if (!ModelState.IsValid)
                return View(validateEmail);

            // Confirm email, skip BL for simplicity
            _userRepo.ConfirmEmail(
                validateEmail.Email,
                validateEmail.SecurityToken);

            return RedirectToAction("Video", "Video");

        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(VMLogin login, bool staySignedIn)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            try
            {
                var user = _userRepo.GetConfirmedUser(login.Username, login.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                    return View(login);
                }

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authenticationProperties = new AuthenticationProperties();

                if (staySignedIn)
                {
                    // Set a future expiration time for the authentication cookie
                    authenticationProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30);
                }

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authenticationProperties).Wait();

                return RedirectToAction("Video", "Video");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(login);
            }
        }




        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction("Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(VMChangePassword changePassword)
        {
            // Change user password, skip BL for simplicity
            _userRepo.ChangePassword(
                changePassword.Username,
                changePassword.NewPassword);

            return RedirectToAction("Video", "Video");

        }

        private string GenerateActivationCode()
        {
            return Guid.NewGuid().ToString();
        }

    }
}

