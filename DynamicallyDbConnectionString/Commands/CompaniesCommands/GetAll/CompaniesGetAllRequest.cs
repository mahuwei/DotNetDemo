using System.Collections.Generic;
using DynamicallyDbConnectionString.Models;
using MediatR;

namespace DynamicallyDbConnectionString.Commands.CompaniesCommands.GetAll {
  public class CompaniesGetAllRequest : IRequest<IEnumerable<Company>> {
  }
}