using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TweetBook.Contracts.V1;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class TagsController : Controller
    {
        private readonly IPostService postService;

        public TagsController(IPostService postService)
        {
            this.postService = postService;
        }

        [Authorize(Policy = "TagViewer")]
        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await this.postService.GetAllTagsAsync();

            return Ok(tags);
        }
    }
}