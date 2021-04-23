using System;
using System.Collections;
using System.Linq;

namespace DynamicallyDbConnectionString.Customs {
  /// <summary>
  ///   ApiController统一返回结果
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ApiResponse<T> {
    /// <summary>
    ///   ctor
    /// </summary>
    public ApiResponse() {
      Status = 0;
    }

    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="data"></param>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    /// <param name="ex"></param>
    public ApiResponse(T? data, int status = 0, string msg = "Ok", Exception? ex = null) {
      Status = status;
      Msg = msg;
      Data = data;
      if (status == 0 && data != null) {
        var type = typeof(T)!;
        IsList = data is IEnumerable;
        if (IsList) {
          var first = type.GenericTypeArguments.First();
          EntityName = first.Name;
          EntityFullName = first.FullName;
        }
        else {
          EntityName = type.Name;
          EntityFullName = type.FullName;
        }
      }

      if (status == 0) {
        return;
      }

      if (ex == null) {
        return;
      }

      ErrorDetail = BadRequestMessage.CreateMessage(ex, out var errMsg);
      Msg = errMsg;
    }

    /// <summary>
    ///   实体类型名称
    /// </summary>
    public string? EntityName { get; set; }

    /// <summary>
    ///   实体类型全称
    /// </summary>
    public string? EntityFullName { get; set; }

    /// <summary>
    ///   是否是数组
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    ///   操作成功失败的最终标示：0=成功；!0=失败
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    ///   错误信息（成功时忽略）
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    ///   返回数据内容
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    ///   错误详情
    /// </summary>
    public BadRequestMessage? ErrorDetail { get; set; }
  }
}