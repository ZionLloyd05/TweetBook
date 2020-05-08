using System.Collections.Generic;

namespace TweetBook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }

        public List<CreateTagDto> Tags { get; set; }
    }
}