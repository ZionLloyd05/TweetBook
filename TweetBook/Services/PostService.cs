using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext context;

        public PostService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await this.context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await this.context.Posts.SingleOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await this.context.Posts.AddAsync(post);
            var created = await this.context.SaveChangesAsync();

            return created > 0;

        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            this.context.Posts.Update(postToUpdate);

            var updated = await this.context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            this.context.Posts.Remove(post);
            var deleted = await this.context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await this.context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}