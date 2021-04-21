using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DynamicallyDbConnectionString.Models {
  public class DemoDbContext : DbContext {
    private static readonly ConcurrentBag<string> CompanyIds = new ConcurrentBag<string>();

    /// <summary>
    ///   默认构造函数
    /// </summary>
    public DemoDbContext() {
    }

    /// <summary>
    ///   导入配置构造函数
    /// </summary>
    /// <param name="options"></param>
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options) {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Employee> Employees => Set<Employee>();

    public void MigrationCustom(string cid) {
      if (string.IsNullOrEmpty(cid)) {
        throw new ArgumentNullException(nameof(cid));
      }

      if (CompanyIds.Any(c => c == cid)) {
        return;
      }

      base.Database.Migrate();
      CompanyIds.Add(cid);
    }
  }
}