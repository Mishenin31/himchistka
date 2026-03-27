using Himchistka.Core.Models;
using Himchistka.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Data.Repositories;

public class ServiceRepository : Repository<Service>
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Service>> SearchAsync(string query) =>
        await Context.Services.Where(x => x.Name.Contains(query) || x.ClothingType.Contains(query)).ToListAsync();

    public async Task<IReadOnlyList<Service>> GetByCategoryAsync(string category) =>
        await Context.Services.Where(x => x.Category == category && x.IsActive).ToListAsync();

    public async Task<IReadOnlyList<Service>> GetActiveAsync() =>
        await Context.Services.Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync();
}
