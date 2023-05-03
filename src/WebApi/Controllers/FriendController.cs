using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("friend")]
    [Authorize]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;

        public FriendController(
            IUserRepository userRepository,
            IFriendRepository friendRepository)
        {
            _userRepository = userRepository;
            _friendRepository = friendRepository;
        }


        [HttpPut("set/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Set([FromRoute]Guid userId)
        {
            var exist = await _userRepository.UserExist(userId, HttpContext.RequestAborted);
            if (!exist)
            {
                return NotFound();
            }

            var meId = GetMyId();

            await _friendRepository.SetFriend(meId, userId, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpPut("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute]Guid userId)
        {
            var meId = GetMyId();

            var deleted = await _friendRepository.DeleteFriend(meId, userId, HttpContext.RequestAborted);
            if (!deleted)
            {
                return NotFound("You are not friends");
            }

            return Ok();
        }

        private Guid GetMyId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
