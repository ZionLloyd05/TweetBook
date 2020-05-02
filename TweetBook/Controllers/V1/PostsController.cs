using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TweetBook.Contracts.V1;
using TweetBook.Domain;

namespace TweetBook.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> posts;

        public PostsController()
        {
            this.posts = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                this.posts.Add(new Post{Id = Guid.NewGuid().ToString()});
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(this.posts);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] Post post)
        {
            if (string.IsNullOrEmpty(post.Id))
                post.Id = Guid.NewGuid().ToString();

            this.posts.Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);
            return Created(locationUri, post);
        }
    }
}