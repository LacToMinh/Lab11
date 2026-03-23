using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASC.DataAccess
{
  public class UnitOfWork : IUnitOfWork
  {
    private Dictionary<string, object> _repositories;
    private DbContext _dbContext;

    public UnitOfWork(DbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public int CommitTransaction()
    {
      return _dbContext.SaveChanges();
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        _dbContext.Dispose();
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    // Khởi tạo Repository tự động (Reflection)
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
      if (_repositories == null)
        _repositories = new Dictionary<string, object>();

      var type = typeof(T).Name;

      // Nếu đã có trong cache thì lấy ra dùng luôn
      if (_repositories.ContainsKey(type))
        return (IRepository<T>)_repositories[type];

      // Nếu chưa có, dùng Activator để tạo instance mới và lưu vào Dictionary
      var repositoryType = typeof(Repository<>);
      var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
      _repositories.Add(type, repositoryInstance);

      return (IRepository<T>)_repositories[type];
    }
  }
}