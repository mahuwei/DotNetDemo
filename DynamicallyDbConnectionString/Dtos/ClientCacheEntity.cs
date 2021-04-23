using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DynamicallyDbConnectionString.Models;

namespace DynamicallyDbConnectionString.Dtos {
  /// <summary>
  ///   基础数据本地缓存结构定义
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ClientCacheEntity<T> where T : Entity {
    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="entities"></param>
    public ClientCacheEntity(List<T>? entities) {
      var type = typeof(T);
      EntityName = type.Name;
      EntityFullName = type.FullName;
      Entities = new ConcurrentDictionary<Guid, T>();
      if (entities == null || entities.Any()) {
        return;
      }

      foreach (var entity in entities) {
        Entities.TryAdd(entity.Id, entity);
      }
    }

    /// <summary>
    ///   实体名称
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    ///   实体全名，可以通过本项反射出相应的数据类型
    /// </summary>
    public string? EntityFullName { get; set; }

    /// <summary>
    ///   数据集合
    /// </summary>
    public ConcurrentDictionary<Guid, T> Entities { get; set; }

    /// <summary>
    ///   修改记录
    /// </summary>
    /// <param name="dto"></param>
    public void UpdateEntity(EntityChangeDto dto) {
      var entities = new List<T>();
      if (dto.IsList == false) {
        var changeEntity = JsonSerializer.Deserialize<T>(dto.Entities);
        if (changeEntity == null) {
          return;
        }

        entities.Add(changeEntity);
      }
      else {
        var changeEntities = JsonSerializer.Deserialize<List<T>>(dto.Entities);
        if (changeEntities == null) {
          return;
        }

        entities.AddRange(changeEntities);
      }

      foreach (var entity in entities) {
        Entities.AddOrUpdate(entity.Id, entity, (id, old) => entity);
      }
    }
  }
}