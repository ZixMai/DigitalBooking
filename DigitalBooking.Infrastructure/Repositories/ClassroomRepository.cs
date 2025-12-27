using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class ClassroomRepository(DbContext dbContext) : IClassroomRepository
{
    public async Task<List<Classroom>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Classrooms
            .Include(classroom => classroom.OwnerDepartment)
            .Include(classroom => classroom.ClassroomType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Classroom?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Classrooms
            .Include(classroom => classroom.OwnerDepartment)
            .Include(classroom => classroom.ClassroomType)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    
    public async Task<ClassroomType?> GetTypeAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.ClassroomTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    
    public async Task<ClassroomType?> GetTypeAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.ClassroomTypes.FirstOrDefaultAsync(e => e.TypeName == name, cancellationToken);
    }

    public async Task<Classroom?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Classrooms.FirstOrDefaultAsync(e => e.Title == name, cancellationToken);
    }
    
    public async Task CreateAsync(Classroom entity, CancellationToken cancellationToken)
    {
        await dbContext.Classrooms.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Classroom entity, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Classrooms
            .FirstAsync(classroom => classroom.Id == entity.Id, cancellationToken);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Classrooms
            .Where(classroom => classroom.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
