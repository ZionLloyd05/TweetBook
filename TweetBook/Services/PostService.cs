using System;
using System.Collections.Generic;
using System.Linq;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly List<Post> posts;

        public PostService()
        {
            this.posts = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                this.posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post name {i}"
                });
            }
        }

        public List<Post> GetPosts()
        {
            return this.posts;
        }

        public Post GetPostById(Guid postId)
        {
            return this.posts.SingleOrDefault(p => p.Id == postId);
        }

        public bool UpdatePost(Post postToUpdate)
        {
            var exists = GetPostById(postToUpdate.Id) != null;

            if (!exists)
                return false;

            var index = this.posts.FindIndex(x => x.Id == postToUpdate.Id);
            this.posts[index] = postToUpdate;
            return true;
        }

        public bool DeletePost(Guid postId)
        {
            var exists = GetPostById(postId) != null;

            if (!exists)
                return false;

            var index = this.posts.FindIndex(x => x.Id == postId);
            this.posts.RemoveAt(index);
            return true;
        }
    }
}