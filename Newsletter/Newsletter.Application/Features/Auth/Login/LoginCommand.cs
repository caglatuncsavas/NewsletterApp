using CTS.Result;
using MediatR;

namespace Newsletter.Application.Features.Auth.Login;
public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : IRequest<Result<string>>;
