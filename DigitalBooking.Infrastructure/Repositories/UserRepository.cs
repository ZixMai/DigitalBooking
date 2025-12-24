using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class UserRepository(DbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Username == userEmail, cancellationToken: cancellationToken);
    }

    public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user,  cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken: cancellationToken);
    }
}