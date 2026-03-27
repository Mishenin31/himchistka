using Himchistka.Core.Enums;
using Himchistka.Core.Models;
using Himchistka.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Data.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> FindByLoginAsync(string login) =>
        await Context.Users.FirstOrDefaultAsync(x => x.Email == login || x.Phone == login);

    public async Task<IReadOnlyList<User>> GetClientsAsync() =>
        await Context.Users.Where(x => x.Role == UserRole.Client).OrderBy(x => x.FullName).ToListAsync();

    public async Task<IReadOnlyList<User>> GetLoyalClientsAsync() =>
        await Context.Users.Where(x => x.Role == UserRole.Client && x.OrdersCount > 10).ToListAsync();
}
