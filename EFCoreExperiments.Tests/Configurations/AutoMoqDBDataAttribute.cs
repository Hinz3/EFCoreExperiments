using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EFCoreExperiments.Tests.Configurations.Customizations;

namespace EFCoreExperiments.Tests.Configurations;

/// <summary>
/// AutoMoqData using EF Core SQL Server to be used for integration tests. For testing using a docker MSSQL Server.
/// NOTE: Will not delete the database afterwards
/// </summary>
public class AutoMoqDBDataAttribute : AutoDataAttribute
{
    public AutoMoqDBDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize(new AutoMoqCustomization());
        fixture.Customize(new MSSQLEntityFrameworkCustomization());

        return fixture;
    }
}
