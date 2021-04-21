using DynamicallyDbConnectionString.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicallyDbConnectionString.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class DbTestController : ControllerBase {
    private readonly DemoDbContext _dc;

    public DbTestController(DemoDbContext dc) {
      _dc = dc;
    }

    [HttpGet]
    public string Get() {
      return _dc.Database.GetConnectionString();
    }
  }
}