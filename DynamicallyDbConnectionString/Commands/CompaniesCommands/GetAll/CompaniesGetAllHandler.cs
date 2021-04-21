using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicallyDbConnectionString.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicallyDbConnectionString.Commands.CompaniesCommands.GetAll {
  public class CompaniesGetAllHandler : IRequestHandler<CompaniesGetAllRequest, IEnumerable<Company>> {
    private readonly DemoDbContext _dc;

    public CompaniesGetAllHandler(DemoDbContext dc) {
      _dc = dc;
    }

    public async Task<IEnumerable<Company>>
      Handle(CompaniesGetAllRequest request, CancellationToken cancellationToken) {
      return await _dc.Companies.AsNoTracking().ToListAsync(cancellationToken);
    }
  }
}