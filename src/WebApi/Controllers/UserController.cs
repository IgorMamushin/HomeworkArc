using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.HttpModels;

namespace WebApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetuserById([FromRoute] Guid id)
        {
            var user = await _repository.GetUser(id, HttpContext.RequestAborted);
            if (user == null)
                return NotFound();

            var response = new GetUserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age, 
                Biography = user.Biography,
                City = user.City
            };

            return Ok(response);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var user = new Models.User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Biography= request.Biography,
                City= request.City,
                PasswordHash = request.Password
            };
            var userId = await _repository.CreateUser(user, HttpContext.RequestAborted);
            return Ok(new RegisterUserResponse()
            {
                Id = userId
            });
        }
    }
}
