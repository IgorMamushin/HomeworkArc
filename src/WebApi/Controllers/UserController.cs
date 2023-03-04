using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    public class GetUserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Biography { get; set; }
        public string City { get; set; }
    }

    public class RegisterUserRequest
    {
        [Required, MinLength(1), MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        [MaxLength(1000)]
        public string Biography { get; set; }
        [Required, MinLength(1), MaxLength(30)]
        public string City { get; set; }

        [Required, MinLength(1)]
        public string Password { get;set; }
    }

    public class RegisterUserResponse
    {
        public Guid Id { get; set; }
    }

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
