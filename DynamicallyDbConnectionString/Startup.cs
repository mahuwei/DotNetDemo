using System;
using System.Linq;
using DynamicallyDbConnectionString.Customs;
using DynamicallyDbConnectionString.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DynamicallyDbConnectionString {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1",
          new OpenApiInfo { Title = "DynamicallyDbConnectionString", Version = "v1" });
      });


      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddTransient(CreateDbContext);

      services.AddMediatR(typeof(Startup).Assembly);

      // 全局注册异常过滤器
      services.AddControllersWithViews(option => {
        option.Filters.Add<ResultFilterAsync>();
        //option.Filters.Add<ResultFilter>();
        //option.Filters.Add<ActionFilter>();
      });
    }

    private DemoDbContext CreateDbContext(IServiceProvider options) {
      var connectionStringPlaceHolder = Configuration.GetConnectionString("Default");
      var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
      var cid = httpContextAccessor?.HttpContext?.Request.Headers["tenantId"].First();
      if (string.IsNullOrEmpty(cid)) {
        cid = "base";
      }

      var connectionString = connectionStringPlaceHolder.Replace("{cid}", cid);
      var builder = new DbContextOptionsBuilder<DemoDbContext>();
      builder.UseSqlServer(connectionString);
      var dbContext = new DemoDbContext(builder.Options);
      dbContext.MigrationCustom(cid);
      return dbContext;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicallyDbConnectionString v1"));
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}