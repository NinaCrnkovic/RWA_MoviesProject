using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using BL.Services;
using IntegrationModule.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly ICountryRepository _countryRepo;

        private readonly IJwtService _jwtService;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository userRepo, ICountryRepository countryRepo, IJwtService jwtService)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepo = userRepo;
            _jwtService = jwtService;
            _countryRepo = countryRepo;
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            try
            {
                var blUsers = _userRepo.GetAll();
                var users = _mapper.Map<IEnumerable<User>>(blUsers);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the users.");
            }
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var blUser = _userRepo.GetById(id);
                if (blUser == null)
                {
                    return NotFound();
                }
                var user = _mapper.Map<User>(blUser);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching the user with ID {id}.");
            }
        }

        [HttpPost("[action]")]
     
        public IActionResult Create(CreateUserRequest request)
        {
            try
            {
                var country = _countryRepo.GetById(request.CountryId);
                if (country == null)
                {
                    return BadRequest("Invalid country ID.");
                }

                var createdUser = _userRepo.CreateUserMVC(request.Username, request.FirstName, request.LastName, request.Email, request.Phone, request.Password, request.CountryId);
                var createdUserDto = _mapper.Map<User>(createdUser);

                // Generiraj JWT token za stvorenog korisnika
                var jwtToken = _jwtService.GenerateJwtToken(createdUserDto.Email);

                return CreatedAtAction(nameof(GetById), new { id = createdUserDto.Id }, new { User = createdUserDto, Token = jwtToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
            }
        }




        [HttpPut("[action]/{id}")]
        public IActionResult Update(int id, User user)
        {
            try
            {
                var blUser = _userRepo.GetById(id);
                if (blUser == null)
                {
                    return NotFound();
                }
                _mapper.Map(user, blUser);
                _userRepo.UpdateUser(blUser.Id, blUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the user with ID {id}.");
            }
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var blUser = _userRepo.GetById(id);
                if (blUser == null)
                {
                    return NotFound();
                }
                _userRepo.SoftDeleteUser(blUser.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the user with ID {id}.");
            }
        }
    }
}



//[HttpPost("[action]")]
//public ActionResult<User> Register([FromBody] Models.UserRegisterRequest request)
//{
//    if (!ModelState.IsValid)
//        return BadRequest(ModelState);

//    try
//    {
//        var newUser = _userRepo.Add(request);

//        return Ok(new Models.UserRegisterResponse
//        {
//            Id = newUser.Id,
//            SecurityToken = newUser.SecurityToken
//        });
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}

//[HttpPost("[action]")]
//public ActionResult ValidateEmail([FromBody] Models.ValidateEmailRequest request)
//{
//    try
//    {
//        _userRepo.ValidateEmail(request);
//        return Ok();
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}

//[HttpPost("[action]")]
//public ActionResult<Models.Tokens> JwtTokens([FromBody] Models.JwtTokensRequest request)
//{
//    try
//    {
//        return Ok(_userRepo.JwtTokens(request));
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}

//[HttpPost("[action]")]
//public ActionResult ChangePassword([FromBody] Models.ChangePasswordRequest request)
//{
//    try
//    {
//        _userRepo.ChangePassword(request);
//        return Ok();
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}



