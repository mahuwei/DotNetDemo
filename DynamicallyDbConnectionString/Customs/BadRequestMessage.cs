using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicallyDbConnectionString.Customs {
  /// <summary>
  ///   错误消息
  /// </summary>
  public class BadRequestMessage {
    /// <summary>
    ///   错误类型
    /// </summary>
    public EnumBadRequestMessageType MessageType { get; set; }

    /// <summary>
    ///   错误消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///   创建错误对象
    /// </summary>
    /// <param name="e"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static BadRequestMessage CreateMessage(Exception e, out string message) {
      BadRequestMessage bm;
      if (e is ValidationException ev) {
        bm = new BadRequestMessage { MessageType = EnumBadRequestMessageType.Validate, Message = ev.Message };
        message = "校验错误，查看详细信息。";
      }
      else {
        bm = new BadRequestMessage { MessageType = EnumBadRequestMessageType.Other, Message = e.Message };
        message = e.Message;
      }

      return bm;
    }
  }
}