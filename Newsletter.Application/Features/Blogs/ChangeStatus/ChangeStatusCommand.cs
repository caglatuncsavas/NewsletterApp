using CTS.Result;
using MediatR;

namespace Newsletter.Application.Features.Blogs.ChangeStatus;
public sealed record ChangeStatusCommand(
      Guid Id) : IRequest<Result<string>>;
  