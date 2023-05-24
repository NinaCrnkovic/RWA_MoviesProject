using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using IntegrationModule.Mapping;
//using BL.DALModels;
using IntegrationModule.Models;
using IntegrationModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//namespace IntegrationModule.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly ILogger<UserController> _logger;

//        private readonly IMapper _mapper;
//        private readonly IUserRepository _userRepo;

//        public UserController(Logger<UserController> logger, IMapper mapper, IUserRepository userRepo)
//        {
 
//            _mapper = mapper;
//            _logger = logger;
//            _userRepo = userRepo;
//        }

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            try
//            {
//                var blUsers = _userRepo.GetAll();
//                var users = _mapper.Map<IEnumerable<User>>(blUsers);
//                return Ok(users);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while fetching the users.");
//                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the users.");
//            }
//        }

//        [HttpPost("[action]")]
//        public ActionResult<User> Register([FromBody] UserRegisterRequest request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var newUser = _userRepo.Add(request);

//                return Ok(new UserRegisterResponse
//                {
//                    Id = newUser.Id,
//                    SecurityToken = newUser.SecurityToken
//                });
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("[action]")]
//        public ActionResult ValidateEmail([FromBody] ValidateEmailRequest request)
//        {
//            try
//            {
//              //  _userRepository.ValidateEmail(request);
//                return Ok();
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("[action]")]
//        public ActionResult<Tokens> JwtTokens([FromBody] JwtTokensRequest request)
//        {
//            try
//            {
//               // return Ok(_userRepository.JwtTokens(request));
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("[action]")]
//        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
//        {
//            try
//            {
//               // _userRepository.ChangePassword(request);
//                return Ok();
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
////        }


//    }    
//}
