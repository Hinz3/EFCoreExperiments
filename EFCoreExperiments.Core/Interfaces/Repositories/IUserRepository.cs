using EFCoreExperiments.DataContext.Entities;
using EFCoreFiltering.Models;

namespace EFCoreExperiments.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<PagedResponse<User>> GetUsers();
}
