using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicallyDbConnectionString.Commands.CompaniesCommands.GetAll;
using DynamicallyDbConnectionString.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicallyDbConnectionString.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class CompaniesController : ControllerBase {
    private readonly ILogger<CompaniesController> _logger;
    private readonly IMediator _mediator;

    public CompaniesController(ILogger<CompaniesController> logger, IMediator mediator) {
      _logger = logger;
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Company>> Get() {
      var request = new CompaniesGetAllRequest();
      return await _mediator.Send(request);
    }
  }
}