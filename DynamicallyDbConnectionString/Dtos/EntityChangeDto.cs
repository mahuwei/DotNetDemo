namespace DynamicallyDbConnectionString.Dtos {
  /// <summary>
  ///   基础数据变更记录
  /// </summary>
  public class EntityChangeDto {
    /// <summary>
    ///   实体名称
    /// </summary>
    public string EntityName { get; set; } = null!;

    /// <summary>
    ///   企业编号
    /// </summary>
    public string CompanyNo { get; set; } = null!;

    /// <summary>
    ///   装配件名称，可以通过本项反射出相应的数据类型
    /// </summary>
    public string EntityFullName { get; set; } = null!;

    /// <summary>
    ///   是否是数组
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    ///   变更数据记录集
    ///   内容：变更实体集合+json序列化后的字符串
    /// </summary>
    public string Entities { get; set; } = null!;
  }
}