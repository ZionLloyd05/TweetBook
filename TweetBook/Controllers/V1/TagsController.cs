using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Poster")]
    public class TagsController : Controller
    {
        private readonly IPostService postService;

        public TagsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await this.postService.GetAllTagsAsync();

            var tagResponses = tags.Select(tag => new TagResponse
            {
                Name = tag.Name
            });

            return Ok(tagResponses);
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            var tag = await this.postService.GetTagByNameAsync(tagName);

            if (tag == null)
                return NotFound();

            var tagResponse = new TagResponse
            {
                Name = tag.Name
            };

            return Ok(tagResponse);
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest tagRequest)
        {
            var newTag = new Tag
            {
                Name = tagRequest.Name,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await this.postService.CreateTagAsync(newTag);

            if (!created)
            {
                return BadRequest("Unable to create tag");
            }

            var tagResponse = new TagResponse
            {
                Name = newTag.Name
            };

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);
            return Created(locationUri, tagResponse);
        }
    }
}