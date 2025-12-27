using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class DepartmentRepository(DbContext dbContext) : IDepartmentRepository
{
    public async Task<List<Department>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Departments.ToListAsync(cancellationToken);
    }

    public async Task<Department?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Departments.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Departments.FirstOrDefaultAsync(e => e.Title == name, cancellationToken);
    }
    
    public async Task CreateAsync(Department entity, CancellationToken cancellationToken)
    {
        await dbContext.Departments.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Department entity, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Departments
            .FirstAsync(department => department.Id == entity.Id, cancellationToken);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Departments
            .Where(department => department.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
