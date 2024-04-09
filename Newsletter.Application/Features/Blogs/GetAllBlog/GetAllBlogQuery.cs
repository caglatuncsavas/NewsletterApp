using CTS.Result;
using MediatR;
using Newsletter.Domain.Entities;

namespace Newsletter.Application.Features.Blogs.GetAllBlog;
public sealed record GetAllBlogQuery(
    string Search) : IRequest<Result<List<Blog>>>;
