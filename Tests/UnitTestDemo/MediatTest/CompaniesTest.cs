using System.Linq;
using System.Threading;
using DynamicallyDbConnectionString.Commands.CompaniesCommands.GetAll;
using Xunit;

namespace UnitTestDemo.MediatTest {
  public class CompaniesTest : IClassFixture<SharedDatabaseFixture> {
    public CompaniesTest(SharedDatabaseFixture fixture) {
      Fixture = fixture;
    }

    public SharedDatabaseFixture Fixture { get; }

    [Fact]
    public async void GetAll() {
      // 启用事务时需要先创建事务对象
      await using var tran = await Fixture.Connection.BeginTransactionAsync();
      await using var demoDbContext = Fixture.CreateContext(tran);
      var companiesGetAllHandler = new CompaniesGetAllHandler(demoDbContext);
      var result =
        await companiesGetAllHandler.Handle(new CompaniesGetAllRequest(), CancellationToken.None);

      Assert.NotNull(result);
      Assert.Equal(5, result.Count());
    }
  }
}