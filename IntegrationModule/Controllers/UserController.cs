using BL.DALModels;
using IntegrationModule.Models;
using IntegrationModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userRepository;

        public UserController(IUserService userRepo)
        {
            _userRepository = userRepo;
        }

        [HttpPost("[action]")]
        public ActionResult<BL.DALModels.User> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newUser = _userRepository.Add(request);

                return Ok(new UserRegisterResponse
                {
                    Id = newUser.Id,
                    SecurityToken = newUser.SecurityToken
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ValidateEmail([FromBody] ValidateEmailRequest request)
        {
            try
            {
                _userRepository.ValidateEmail(request);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<Tokens> JwtTokens([FromBody] JwtTokensRequest request)
        {
            try
            {
                return Ok(_userRepository.JwtTokens(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                _userRepository.ChangePassword(request);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
