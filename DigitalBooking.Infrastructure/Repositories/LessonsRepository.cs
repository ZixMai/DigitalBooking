using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class LessonsRepository(DbContext dbContext) : ILessonsRepository
{
    public async Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Lessons
            .Include(e => e.Discipline)
            .Include(e => e.Teacher)
                .ThenInclude(teacher => teacher.Department)
            .Include(e => e.Group)
            .ToListAsync(cancellationToken);
    }

    public async Task<Lesson?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Lessons
            .Include(e => e.Discipline)
            .Include(e => e.Teacher)
                .ThenInclude(teacher => teacher.Department)
            .Include(e => e.Group)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Lesson entity, CancellationToken cancellationToken)
    {
        await dbContext.Lessons.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(long id, string link, CancellationToken cancellationToken)
    {
        await dbContext.Lessons
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.LmsLink, link), cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Lessons
            .Where(lesson => lesson.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
