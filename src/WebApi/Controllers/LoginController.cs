using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.HttpModels;

namespace WebApi.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _repository;

        public LoginController(ILoginRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _repository.Login(request.Id, request.Password, HttpContext.RequestAborted);
            if (!token.HasValue)
            {
                return NotFound();
            }

            return Ok(new LoginResponse()
            {
                Token = token.Value
            });
        }

    }
}
