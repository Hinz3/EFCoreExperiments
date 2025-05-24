using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EFCoreExperiments.DataContext.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExperiments.Tests.Configurations;

/// <summary>
/// AutoMoqData using EF Core InMemoryDatabase for unit testing DbContext
/// </summary>
public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        var options = new DbContextOptionsBuilder<MainContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new MainContext(options);

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Register<MainContext>(() => context);
        fixture.Customize(new AutoMoqCustomization());

        return fixture;
    }
}
