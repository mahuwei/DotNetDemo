using System.ComponentModel;

namespace DynamicallyDbConnectionString.Customs {
  /// <summary>
  ///   错误类型
  /// </summary>
  [Description("错误类型")]
  public enum EnumBadRequestMessageType {
    /// <summary>
    ///   数据校验错误
    /// </summary>
    [Description("数据验证错误")]
    Validate = 0,

    /// <summary>
    ///   业务执行错误；应该调用不满足条件
    /// </summary>
    [Description("业务执行错误")]
    MediatorHandle = 10,

    /// <summary>
    ///   其他错误
    /// </summary>
    [Description("其他错误")]
    Other = 100
  }
}