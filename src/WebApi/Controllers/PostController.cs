using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using WebApi.HttpModels;

namespace WebApi.Controllers
{
    [Route("post")]
    [Authorize]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _repository;

        public PostController(IPostRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatePostResponse))]
        public async Task<IActionResult> Create([FromBody]PostRequest request)
        {
            var userId = GetMyId();
            var postId = await _repository.Create(userId, request.Text, HttpContext.RequestAborted);

            return Ok(new CreatePostResponse
            {
                PostId = postId
            });
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Delete([FromRoute]long id)
        {
            var userId = GetMyId();
            await _repository.Delete(userId, id, HttpContext.RequestAborted);

            return Ok();
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Update([FromRoute]long id, [FromBody]PostRequest request)
        {
            var userId = GetMyId();
            await _repository.Update(userId, id, request.Text, HttpContext.RequestAborted);

            return Ok();
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Get([FromRoute]long id)
        {
            var post = await _repository.Get(id, HttpContext.RequestAborted);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost("feed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]

        public async Task<IActionResult> Feed([FromBody] FeedRequest request)
        {
            var userId = GetMyId();
            var posts = await _repository.Feed(userId, request.Offset ?? 0, request.Limit ?? 10, HttpContext.RequestAborted);

            return Ok(posts);
        }

        private Guid GetMyId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
