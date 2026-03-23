using ASC.Model.BaseTypes;
using System;

namespace ASC.DataAccess.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IRepository<T> Repository<T>() where T : BaseEntity;
    int CommitTransaction();
  }
}