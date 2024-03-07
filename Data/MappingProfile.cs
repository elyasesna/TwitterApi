using AutoMapper;
using System.Reflection;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;

namespace TwitterApi.Data
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserDTO>()
				.ForMember(p => p.IsConfirmed,
							  q => q.MapFrom(x => x.EmailConfirmed || x.PhoneNumberConfirmed));

			CreateMap<Post, PostDTO>()
				.ForMember(p => p.Username,
							  q => q.MapFrom(x => x.User.UserName))
				.ForMember(p => p.CommentCount,
							  q => q.MapFrom(x => x.Comments.Count()))
				.ForMember(p => p.LikeCount,
							  q => q.MapFrom(x => x.Likes.Count()))
				.ForMember(p => p.VisitCount,
							  q => q.MapFrom(x => x.Visitors.Count()))
				.ForMember(p => p.Hashtags,
							  q => q.MapFrom(x => x.Hashtags.Select(p => p.Hashtag.Tag)));
		}
	}
}











//.ForMember(dest => dest.Avatar, opt => opt.ExplicitExpansion());
//.ForMember(
//        dest => dest.HasPost,
//        opt => opt.MapFrom(source => source.Posts.Any())
//    );


//var types = Assembly.GetExecutingAssembly().GetTypes()
//           .Where(type => !string.IsNullOrEmpty(type.Namespace) &&
//                           type.BaseType != null &&
//                           type.BaseType == typeof(Bar));
//foreach (Type type in types)
//{
//   CreateMap(type, typeof(BarContract));
//}