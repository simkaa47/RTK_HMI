﻿using DataAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T>, IDisposable where T : class, IDatabased
    {
        public readonly ApplicationContext _dbContext;
        public EfRepository()
        {
            _dbContext = new ApplicationContext();
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            T entityFromDb = _dbContext.Set<T>().Find(entity.Id);
            if (entityFromDb != null)
            {
                _dbContext.Set<T>().Remove(entityFromDb);
                _dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }

        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>()
                 .Where(e => e.Id == id)
                 .FirstOrDefault();

        }

        public IEnumerable<T> Init(IEnumerable<T> collection)
        {
            return _dbContext.Init(collection);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
