using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, IUserRepository userRepo, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;  
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var blUsers = _userRepo.GetAll();
            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);
            return View(vmUsers);
        }

        
    }
}