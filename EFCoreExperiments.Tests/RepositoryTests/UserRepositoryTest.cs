using AutoFixture.Xunit2;
using EFCoreExperiments.Core.Repositories;
using EFCoreExperiments.DataContext.Contexts;
using EFCoreExperiments.DataContext.Entities;
using EFCoreExperiments.Tests.Configurations;
using EFCoreFiltering.Interfaces;
using Moq;

namespace EFCoreExperiments.Tests.RepositoryTests;

public class UserRepositoryTest
{
    [Theory]
    [AutoMoqData]
    public async Task GetUsers_InMemory([Frozen] MainContext context, [Frozen] Mock<IRequestQuery> queryParams, List<User> users, UserRepository sut)
    {
        await context.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var expectedAmount = users.Count / 2;
        queryParams.Setup(x => x.Page).Returns(1);
        queryParams.Setup(x => x.PageSize).Returns(expectedAmount);

        var result = await sut.GetUsers();

        Assert.Equal(expectedAmount, result.Data.Count());
        Assert.Equal(expectedAmount, result.PageSize);
    }

    [Theory]
    [AutoMoqDBData]
    public async Task GetUsers_MSSQL([Frozen] MainContext context, [Frozen] Mock<IRequestQuery> queryParams, List<User> users, UserRepository sut)
    {
        users.ForEach(x => x.Id = 0);

        await context.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var expectedAmount = users.Count / 2;
        queryParams.Setup(x => x.Page).Returns(1);
        queryParams.Setup(x => x.PageSize).Returns(expectedAmount);

        var result = await sut.GetUsers();

        Assert.Equal(expectedAmount, result.Data.Count());
        Assert.Equal(expectedAmount, result.PageSize);
    }
}
