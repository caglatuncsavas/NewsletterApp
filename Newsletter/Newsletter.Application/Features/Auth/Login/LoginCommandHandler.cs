using CTS.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newsletter.Domain.Entities;

namespace Newsletter.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser = 
            await userManager
            .Users
            .FirstOrDefaultAsync(p => 
            p.UserName == request.UserNameOrEmail || 
            p.Email == request.UserNameOrEmail, cancellationToken);

        if(appUser is null)
        {
            return Result<string>.Failure("User not found");
        }

        bool checkPassword = await userManager.CheckPasswordAsync(appUser, request.Password);
        if(!checkPassword)
        {
            return Result<string>.Failure("Password is incorrect");
        }

        return "User logged in successfully";
    }
}
