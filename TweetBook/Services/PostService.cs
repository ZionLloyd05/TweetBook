﻿using Microsoft.EntityFrameworkCore;
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
            return await this.context.Posts
                .Include(x => x.Tags)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await this.context.Posts
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(post);
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

        private async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            {
                var existingTag = await this.context.Tags
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Name == tag.TagName);

                if (existingTag != null)
                    continue;

                await this.context.Tags.AddAsync(
                    new Tag()
                    {
                        Name = tag.TagName,
                        CreatedOn = DateTime.UtcNow,
                        CreatorId = post.UserId
                    });
            }
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await this.context.Tags.AsNoTracking().ToListAsync();
        }

    }
}