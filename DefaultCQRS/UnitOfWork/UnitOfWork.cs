using AlJawad.DefaultCQRS.Enums;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.Models;
using AlJawad.DefaultCQRS.UnitOfWork;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Principal;
using DefaultCQRS.Data;

namespace DefaultCQRS.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T>
        where T : AppDbContext
    {
        protected readonly T DbContext;
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;

        //private Claim User;


        private IEnumerable<EntityEntry> Entries => DbContext.ChangeTracker.Entries();

        public UnitOfWork(T context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void BeginTransaction() => NewTransactionIfNeeded();
        public void CommitTransaction()
        {
            DbContext.SaveChanges();
            if (_transaction == null) return;
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        private void OnBeforeSaving()
        {
            var changedEntries = DbContext.ChangeTracker
                .Entries()
                .Where(e =>
                    (e.Entity is ITrackAudit || e.Entity is IActivation) &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in changedEntries)
            {
                TrackAudit(entry);
                TrackDeleted(entry);
            }
        }
     
        private void TrackAudit(EntityEntry entry)
        {
            if (entry.Entity is ITrackAudit audit)
            {
                if (entry.State == EntityState.Added && audit.CreatedDate == default)
                {
                    audit.CreatedBy =  "System Generated";
                    audit.CreatedDate = DateTimeOffset.UtcNow;
                }
                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    audit.ModifiedBy =  "System Generated";
                    audit.ModifiedDate = DateTimeOffset.UtcNow;
                }

            }
        }
        private void TrackDeleted(EntityEntry entry)
        {
            if (!(entry.Entity is IHardDelete) && entry.Entity is IActivation delete)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["ModifiedDate"] = DateTimeOffset.UtcNow;
                    delete.Status = EntityStatus.Deleted;
                    //foreach (var navigationEntry in entry.Navigations.Where(n => !n.Metadata.IsDependentToPrincipal()))
                    foreach (var navigationEntry in entry.Navigations.Where(n => !((IReadOnlyNavigation)n.Metadata).IsOnDependent))
                    {
                        NavigationEntry(navigationEntry);
                    }

                }
            }
        }
        private void NavigationEntry(NavigationEntry navigationEntry)
        {
            if (navigationEntry is CollectionEntry collectionEntry)
            {
                var currVal = collectionEntry.CurrentValue;
                if (currVal != null)
                {
                    foreach (var dependentEntry in currVal)
                    {
                        if (dependentEntry != null)
                        {
                            HandleDependent(DbContext.Entry(dependentEntry));
                        }
                    }
                }
            }
            else
            {
                var dependentEntry = navigationEntry.CurrentValue;
                if (dependentEntry != null)
                {
                    HandleDependent(DbContext.Entry(dependentEntry));
                }
            }
        }
        private void HandleDependent(EntityEntry entry)
        {
            if (entry.Entity is IActivation child)
            {
                child.Status = EntityStatus.Deleted;
                entry.CurrentValues["ModifiedDate"] = DateTimeOffset.UtcNow;
            }
        }

        public void NewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue ? DbContext.Database.BeginTransaction(_isolationLevel.GetValueOrDefault()) : DbContext.Database.BeginTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                BeginTransaction();
                OnBeforeSaving();
                var result = await DbContext.SaveChangesAsync(cancellationToken);
                return result;

            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new Exception(
                    "The record you attempted to edit was modified by another " +
                    "user after you loaded it. The edit operation was cancelled and the " +
                    "currect values of the record are displayed. Please try again.", exception);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbSet<Y> Set<Y>() where Y : class
        {
            return DbContext.Set<Y>();
        }
        public void Entry<Y>(Y entity, EntityState state) where Y : class
        {
            DbContext.Entry(entity).State = state;
        }
        public void Upsert<Y>(Y entity) where Y : class
        {
            DbContext.ChangeTracker.TrackGraph(entity, e =>
            {


                e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added;

            });
        }
        public async Task<int> SaveChangesNoConflict(CancellationToken cancellationToken)
        {
            OnBeforeSaving();
            var result = await DbContext.SaveChangesAsync(cancellationToken);
            return result;
        }

        public string GetSchema(object entry)
        {
            var schemaAnnotation = DbContext.Model.FindEntityType(entry.GetType()).GetAnnotations()
          .FirstOrDefault(a => a.Name == "Relational:Schema");
            return schemaAnnotation == null ? "dbo" : schemaAnnotation.Value.ToString();
        }

        public CreateResult FindFolder<T1>(T1 Entity) where T1 : class
        {
            //TODO: Implement FindFolder logic
            //NotImplemented
            return null;
        }

        public CreateResult CreateDir<T1>(T1 Entity) where T1 : class
        {
            //TODO: Implement FindFolder logic
            //NotImplemented
            return null;
        }

        public CreateResult CreateFile<T1>(T1 Entity) where T1 : class
        {
            //TODO: Implement FindFolder logic
            //NotImplemented
            return null;
        }

        public List<T1> SPExecute<T1>(object[] parameters, string spName) where T1 : class
        {
            var result = new List<T1>();
            _ = DbContext.Database.ExecuteSqlRaw(spName, parameters);
            return result;
        }

        public CreateResult DeleteFile(string tblName, Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Y> ExecuteSP<Y>(List<KeyValuePair<string, string>> param, string spName) where Y : class
        {
            throw new NotImplementedException();
        }
    }
}


