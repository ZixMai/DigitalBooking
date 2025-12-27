using DigitalBooking.Application.Abstractions;
using DigitalBooking.Application.Services;
using DigitalBooking.Domain;
using DigitalBooking.Presentation.Groups;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace DigitalBooking.Presentation.Endpoints.Features.Auth;


public class RegisterEndpoint(
    IUserRepository userRepository,
    HashService hashService,
    JwtService jwtService
) : Endpoint<RegisterRequest, TokenResponse>
{
    public override void Configure()
    {
        Post("/register");
        Group<AuthGroup>();
        AllowAnonymous();
        
        Description(b => b
            .WithName("Register")
            .WithTags(RouteGroups.Auth)
            .Produces<TokenResponse>());
    }
    
    internal sealed class Validator : Validator<RegisterRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен.")
                .EmailAddress().WithMessage("Email некорректный.")
                .MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен.")
                .MinimumLength(8).WithMessage("Пароль должен быть минимум 8 символов.")
                .MaximumLength(128)
                .Must(p => !p.Any(char.IsWhiteSpace)).WithMessage("Пароль не должен содержать пробелы.")
                .Matches(@"[A-Za-z]").WithMessage("Пароль должен содержать хотя бы одну букву.")
                .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");
        }
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var user = await userRepository.GetUserByEmailAsync(req.Email, ct);
        if (user is not null && user.PasswordHash != string.Empty)
        {
            AddError("User with the same email already exists.");
            await Send.UnauthorizedAsync(ct);
            return;
        }

        if (user?.IsDeleted ?? false)
        {
            AddError("User is deleted.");
            await Send.ForbiddenAsync(ct);
            return;
        }
        user ??= new User { UserEmail = req.Email };
        var passwordHash = hashService.HashPassword(req.Password);
        user.PasswordHash = passwordHash;
        await userRepository.CreateUserAsync(user, ct);
        
        await Send.OkAsync(jwtService.GenerateTokenPair(user), ct);
    }
}