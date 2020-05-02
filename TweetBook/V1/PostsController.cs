using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Domain;

namespace TweetBook.V1
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

        [HttpGet("api/v1/posts")]
        public IActionResult GetAll()
        {
            return Ok(this.posts);
        }
    }
}