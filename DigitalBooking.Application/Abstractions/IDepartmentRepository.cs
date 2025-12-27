using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync(CancellationToken cancellationToken);
    Task<Department?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task CreateAsync(Department entity, CancellationToken cancellationToken);
    Task UpdateAsync(Department entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
