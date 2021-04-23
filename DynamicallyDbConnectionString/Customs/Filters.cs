using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DynamicallyDbConnectionString.Customs {
  /// <summary>
  ///   结果过滤器，可以对结果进行格式化、大小写转换等一系列操作
  /// </summary>
  public class ResultFilter : Attribute, IResultFilter {
    private readonly ILogger<ResultFilter> _logger;

    public ResultFilter(ILogger<ResultFilter> logger) {
      _logger = logger;
    }

    /// <summary>
    ///   在结果执行之后调用的操作
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuted(ResultExecutedContext context) {
      if (context.Exception != null) {
        _logger.LogError($"OnResultExecuted ==> 执行错误:{context.Exception.Message}");
        return;
      }

      var js = context.Result as OkObjectResult;
      if (js == null) {
        _logger.LogError("OnResultExecuted ==> 执行结果为空");
        return;
      }

      var jsValue = js.Value;
      var type = jsValue.GetType();
      var fieldInfos = type.GetProperties();
      var entityNameField = fieldInfos.First(d => d.Name == "EntityName");
      var entityFullNameField = fieldInfos.First(d => d.Name == "EntityFullName");
      var isListField = fieldInfos.First(d => d.Name == "IsList");

      var entityName = entityNameField?.GetValue(jsValue)?.ToString();
      var entityFullName = entityFullNameField?.GetValue(jsValue)?.ToString();
      var isList = isListField?.GetValue(jsValue)?.ToString();

      _logger.LogWarning(
        $"entityName：{entityName}， entityFullName：{entityFullName}， isList：{isList}");


      var apiResponse = jsValue as ApiResponse<dynamic>;
      if (apiResponse == null) {
        _logger.LogError("OnResultExecuted ==> apiResponse类型转换后为空。");
        return;
      }

      if (apiResponse.Status == 0) {
        _logger.LogInformation(
          $"OnResultExecuted ==> 成功,IsList: {apiResponse.IsList}, EntityName:{apiResponse.EntityName}, EntityFullName:{apiResponse.EntityFullName}，结果为:{JsonSerializer.Serialize(apiResponse)}");
      }
      else {
        _logger.LogWarning($"OnResultExecuted ==> 返回失败，错误信息:{apiResponse.Msg}");
      }
    }

    /// <summary>
    ///   在结果执行之前调用的一系列操作
    /// </summary>
    /// <param name="context"></param>
    public void OnResultExecuting(ResultExecutingContext context) {
    }
  }


  ///// <summary>
  /////   可以通过ActionFilter 拦截每个执行的方法进行一系列的操作，比如：执行操作日志、参数验证，权限控制等一系列操作
  ///// </summary>
  //public class ActionFilter : Attribute, IActionFilter {
  //  /// <summary>
  //  ///   执行完成
  //  /// </summary>
  //  /// <param name="context"></param>
  //  public void OnActionExecuted(ActionExecutedContext context) {
  //  }

  //  /// <summary>
  //  ///   执行中...
  //  /// </summary>
  //  /// <param name="context"></param>
  //  public void OnActionExecuting(ActionExecutingContext context) {
  //  }
  //}
}