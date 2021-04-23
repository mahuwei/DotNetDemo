using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DynamicallyDbConnectionString.Dtos;
using DynamicallyDbConnectionString.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DynamicallyDbConnectionString.Commands.Notifications {
  public class FilterNotification : INotification {
    public FilterNotification(EntityChangeDto dto) {
      Dto = dto;
    }

    public EntityChangeDto Dto { get; }
  }

  public class FilterNotificationHandler : INotificationHandler<FilterNotification> {
    private static readonly ClientCacheEntity<Company> ClientCacheEntity =
      new ClientCacheEntity<Company>(null);

    private readonly ILogger<FilterNotificationHandler> _logger;

    public FilterNotificationHandler(ILogger<FilterNotificationHandler> logger) {
      _logger = logger;
    }

    public Task Handle(FilterNotification notification, CancellationToken cancellationToken) {
      _logger.LogInformation("更新数据：{@dto}", notification.Dto);
      ClientCacheEntity.UpdateEntity(notification.Dto);
      _logger.LogInformation("更新后的数据：{@data}", ClientCacheEntity.Entities);
      return Task.CompletedTask;
    }
  }
}