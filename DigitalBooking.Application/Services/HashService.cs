using DigitalBooking.Domain;
using Microsoft.AspNetCore.Identity;

namespace DigitalBooking.Application.Services;

public class HashService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}