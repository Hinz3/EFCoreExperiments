using EFCoreExperiments.Core.Interfaces.Repositories;
using EFCoreExperiments.DataContext.Contexts;
using EFCoreExperiments.DataContext.Entities;
using EFCoreFiltering.Implementations.Extensions;
using EFCoreFiltering.Interfaces;
using EFCoreFiltering.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExperiments.Core.Repositories;

public class UserRepository(MainContext context, IRequestQuery request) : IUserRepository
{
    private readonly MainContext context = context;
    private readonly IRequestQuery request = request;

    public async Task<PagedResponse<User>> GetUsers() => 
        await context.Users.AsNoTracking()
                           .ToListPagedAsync(request);
}
