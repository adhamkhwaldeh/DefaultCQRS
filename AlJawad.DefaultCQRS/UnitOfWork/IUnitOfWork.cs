using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models;

namespace AlJawad.DefaultCQRS.UnitOfWork
{

    public interface IUnitOfWork<T> : IUnitOfWork where T : DbContext
    {

    }
    public interface IUnitOfWork
    {
        DbSet<Y> Set<Y>() where Y : class;
        void Entry<Y>(Y entity, EntityState state) where Y : class;
        void Upsert<Y>(Y entity) where Y : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesNoConflict(CancellationToken cancellationToken);
        void NewTransactionIfNeeded();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();


        string GetSchema(object entry);
        CreateResult FindFolder<Y>(Y Entity) where Y : class;
        CreateResult CreateDir<Y>(Y Entity) where Y : class;
        CreateResult CreateFile<Y>(Y Entity) where Y : class;
        CreateResult DeleteFile(string tblName, Guid id);
        List<Y> ExecuteSP<Y>(List<KeyValuePair<string, string>> param, string spName) where Y : class;

        List<Y> SPExecute<Y>(object[] parameters, string spName) where Y : class;
    }
}
