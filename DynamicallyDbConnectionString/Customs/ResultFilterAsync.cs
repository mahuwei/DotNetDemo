using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicallyDbConnectionString.Commands.Notifications;
using DynamicallyDbConnectionString.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DynamicallyDbConnectionString.Customs {
  public class ResultFilterAsync : Attribute, IAsyncResultFilter {
    private const string CsPostMethod = "POST";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ResultFilterAsync> _logger;
    private readonly IMediator _mediator;

    public ResultFilterAsync(ILogger<ResultFilterAsync> logger,
      IMediator mediator,
      IHttpContextAccessor httpContextAccessor) {
      _logger = logger;
      _mediator = mediator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context,
      ResultExecutionDelegate next) {
      var okObjectResult = context.Result as OkObjectResult;


      try {
        if (context.HttpContext.Request.Path.Value?.Contains(
              $"{SystemConst.MaintainControllerRouteHead}/",
              StringComparison.CurrentCultureIgnoreCase)
            == false) {
          _logger.LogDebug("不是系统维护Controller,{@path}", context.HttpContext.Request.Path.Value);
          return;
        }

        if (context.HttpContext.Request.Method != CsPostMethod) {
          return;
        }

        if (okObjectResult == null) {
          _logger.LogError("OnResultExecutionAsync ==> 执行结果为空");
          return;
        }

        var jsValue = okObjectResult.Value;
        var type = jsValue.GetType();
        var fieldInfos = type.GetProperties();
        var statusField = fieldInfos.First(d => d.Name == "Status");
        if (statusField?.GetValue(jsValue)?.ToString() != "0") {
          var msgField = fieldInfos.First(d => d.Name == "Msg");
          var msg = msgField?.GetValue(jsValue)?.ToString();
          _logger.LogWarning($"OnResultExecutionAsync ==> 运行返回错误:{msg}");
          return;
        }

        var companyNo = _httpContextAccessor.HttpContext?.Request.Headers["companyNo"].First();
        if (string.IsNullOrEmpty(companyNo)) {
          return;
        }

        var entityNameField = fieldInfos.First(d => d.Name == "EntityName");
        var entityFullNameField = fieldInfos.First(d => d.Name == "EntityFullName");
        var isListField = fieldInfos.First(d => d.Name == "IsList");
        var dataField = fieldInfos.First(d => d.Name == "Data");

        var entityName = entityNameField?.GetValue(jsValue)?.ToString();
        var entityFullName = entityFullNameField?.GetValue(jsValue)?.ToString();
        var isList = (bool)(isListField?.GetValue(jsValue) ?? false);
        var data = dataField?.GetValue(jsValue);
        if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(entityFullName)) {
          _logger.LogWarning("entityName或entityFullName为空。{@okObjectResult}", okObjectResult);
          return;
        }

        var dto = new EntityChangeDto {
          EntityName = entityName,
          EntityFullName = entityFullName,
          IsList = isList,
          Entities = JsonSerializer.Serialize(data),
          CompanyNo = companyNo
        };

        await _mediator.Publish(new FilterNotification(dto));

        _logger.LogWarning(
          $"entityName：{entityName}， entityFullName：{entityFullName}， isList：{isList}");
      }
      catch (Exception e) {
        _logger.LogError(e, "处理出错，{@okObjectResult}", okObjectResult);
      }
      finally {
        await next.Invoke();
      }
    }
  }
}