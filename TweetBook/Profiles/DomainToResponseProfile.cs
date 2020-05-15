﻿using AutoMapper;
using System.Linq;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;

namespace TweetBook.Profiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt =>
                    opt.MapFrom(src => src.Tags.Select(x => new TagResponse
                    {
                        Name = x.TagName
                    })));

        CreateMap<Tag, TagResponse>();
        }
    }
}