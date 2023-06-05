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

 

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository userRepo, ICountryRepository countryRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepo = userRepo;
   
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

                var BLCreatedUser = _userRepo.CreateUserMVC(request.Username, request.FirstName, request.LastName, request.Email, request.Phone, request.Password, request.CountryId);
                var createdUser = _mapper.Map<User>(BLCreatedUser);

                // Generiraj JWT token za stvorenog korisnika
                var jwtToken = _userRepo.GenerateJwtToken(createdUser.Email);

                //return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, new { User = createdUser, Token = jwtToken });
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user." );
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user." + ex);
            }
        }




        [HttpPut("[action]/{id}")]
        public IActionResult Update(int id, [FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string email, [FromQuery] string phone, [FromQuery] int countryId)
        {
            try
            {
                var blUser = _userRepo.GetById(id);
                if (blUser == null)
                {
                    return NotFound();
                }
                blUser.FirstName = firstName;
                blUser.LastName = lastName;
                blUser.Email = email;
                blUser.Phone = phone;
                blUser.CountryOfResidenceId = countryId;
         
            
                _userRepo.UpdateUser(blUser.Id, blUser);
                return Ok(blUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the user with ID {id}."+ex);
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
                _userRepo.DeleteUser(blUser.Id);
                return Ok(blUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the user with ID {id}.");
            }
        }
    }
}









