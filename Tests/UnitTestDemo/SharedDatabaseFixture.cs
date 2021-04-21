using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DynamicallyDbConnectionString.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace UnitTestDemo {
  /// <summary>
  ///   测试共享数据库对象
  /// </summary>
  public class SharedDatabaseFixture : IDisposable {
    private static readonly object Lock = new object();
    private static bool _databaseInitialized;

    public SharedDatabaseFixture() {
      Connection =
        new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=DemoDb;ConnectRetryCount=0");

      Seed();

      Connection.Open();
    }

    public DbConnection Connection { get; }

    public void Dispose() {
      Connection.Dispose();
    }

    public DemoDbContext CreateContext(DbTransaction transaction = null) {
      var context = new DemoDbContext(new DbContextOptionsBuilder<DemoDbContext>()
        .UseSqlServer(Connection)
        .Options);

      if (transaction != null) {
        context.Database.UseTransaction(transaction);
      }

      return context;
    }

    private void Seed() {
      lock (Lock) {
        if (_databaseInitialized) {
          return;
        }

        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var companies = new List<Company>();
        for (var i = 0; i < 5; i++) {
          companies.Add(new Company {
            Id = Guid.NewGuid(), No = (i + 1).ToString().PadLeft(3, '0'), Name = $"公司-{i + 1}"
          });
        }

        context.AddRange(companies);

        var firstCompany = companies.First(d => d.No == "002");
        for (var i = 0; i < 5; i++) {
          context.Add(new Employee {
            Id = Guid.NewGuid(),
            CompanyId = firstCompany.Id,
            WorkNo = (2000 + i).ToString(),
            Name = $"员工{2000 + i}",
            MobileNo = $"{13012345678 + i}"
          });
        }

        context.SaveChanges();

        _databaseInitialized = true;
      }
    }
  }
}