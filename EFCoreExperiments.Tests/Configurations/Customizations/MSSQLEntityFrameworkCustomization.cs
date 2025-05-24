using AutoFixture;
using EFCoreExperiments.DataContext.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExperiments.Tests.Configurations.Customizations;

public class MSSQLEntityFrameworkCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // TODO Find a better way to get configuration
        var options = new DbContextOptionsBuilder<MainContext>()
            .UseSqlServer("Server=localhost;Database=EFCoreExperiments_IntegrationTests.DB;User Id=sa;Password=YourStrong@Passw0rd;Encrypt=False")
            .Options;

        var context = new MainContext(options);
        context.Database.EnsureCreated();

        fixture.Register(() => context);
    }
}
