using System;
using System.Collections.Generic;
using System.Linq;
using DynamicallyDbConnectionString.Customs;
using DynamicallyDbConnectionString.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicallyDbConnectionString.Controllers {
  /// <summary>
  ///   用于测试Filter使用
  /// </summary>
  [ApiController]
  [Produces("application/json")]
  [Route("maintain/[controller]/[action]")]
  public class FiltersController : ControllerApiBase {
    private static readonly List<Company> Companies = new List<Company> {
      new Company { Id = Guid.NewGuid(), No = "001", Name = "公司1", LastChange = DateTime.Now },
      new Company { Id = Guid.NewGuid(), No = "002", Name = "公司2", LastChange = DateTime.Now }
    };

    [HttpGet]
    public ActionResult<ApiResponse<List<Company>>> Get(int id) {
      if (id < 0) {
        throw new ArgumentException($"Argument id < 0.id:{id}");
      }

      ChangeTime();

      ApiResponse<List<Company>> response = id < 10
        ? Output(Companies)
        : Output<List<Company>>(null, 1, $"error.Input id:{id}");

      return Ok(response);
    }

    private static void ChangeTime() {
      var now = DateTime.Now;
      foreach (var company in Companies) {
        company.LastChange = now;
      }
    }

    [HttpPost]
    public ActionResult<ApiResponse<Company>> Post(int id) {
      if (id < 0) {
        throw new ArgumentException($"Argument id < 0.id:{id}");
      }

      ChangeTime();

      ApiResponse<Company> response = id < 10
        ? Output(Companies.First())
        : Output<Company>(null, 1, $"error.Input id:{id}");

      return Ok(response);
    }

    [HttpPost]
    public ActionResult<ApiResponse<List<Company>>> PostAll(int id) {
      if (id < 0) {
        throw new ArgumentException($"Argument id < 0.id:{id}");
      }

      ChangeTime();
      return Ok(Output(Companies));
    }
  }
}