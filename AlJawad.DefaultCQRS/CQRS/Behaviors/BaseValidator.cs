using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using AlJawad.DefaultCQRS.Helper;
using AlJawad.DefaultCQRS.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlJawad.DefaultCQRS.Entities;
using AlJawad.DefaultCQRS.Caching;
using AlJawad.DefaultCQRS.Interfaces;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Extensions;

namespace AlJawad.DefaultCQRS.CQRS.Behaviors
{
    public abstract class BaseValidator<T, TEntity, TKey, TReadEntity> : AbstractValidator<T>
         where TEntity :class,IBaseEntity
         where TReadEntity : class
    {
        protected readonly IDistributedCache Cache;
        protected readonly IUnitOfWork _unitOfWork;
        protected DbSet<TEntity> _Entities;
        protected IReadOnlyList<TEntity> ListItems => _EntitiesNoTracking.ToList();
        protected IReadOnlyList<TEntity> CacheItem => (typeof(TEntity).Implements<ICache>()) ?
                                                       Cache.GetAndCacheList(typeof(TReadEntity).Name, () => Task.FromResult(_Entities.AsEnumerable())).Result.ToList() :
                                                      _Entities.ToList();
        protected IQueryable<TEntity> _EntitiesNoTracking => _Entities.AsNoTracking();

        protected BaseValidator()
        {
            _Entities = _unitOfWork.Set<TEntity>();
        }

        protected BaseValidator(IUnitOfWork unitOfWork, IDistributedCache cache)
        {
            Assert.NotNull(unitOfWork, nameof(unitOfWork));
            Assert.NotNull(cache, nameof(cache));
            _unitOfWork = unitOfWork;
            Cache = cache;
            _Entities = _unitOfWork.Set<TEntity>();
        }
    }
}