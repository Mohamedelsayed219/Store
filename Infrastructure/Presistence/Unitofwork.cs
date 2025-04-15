﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Presistence.Data;
using Presistence.Repositories;

namespace Presistence
{
    public class Unitofwork : IUnitofwork
    {
        private readonly StoreDbContext _context;
        //private readonly Dictionary<string, object> _repositories;
        private readonly ConcurrentDictionary<string, object> _repositories;


        public Unitofwork(StoreDbContext context)
        {
            _context = context;
            //_repositories = new Dictionary<string, object>();   
            _repositories = new ConcurrentDictionary<string, object>();   
        }

        //public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if (!_repositories.ContainsKey(type)) 
        //    {
        //        var repository = new GenericRepository<TEntity, TKey>(_context);
        //        _repositories.Add(type, repository);
        //    }

        //    return (IGenericRepository<TEntity, TKey>) _repositories[type];
        //}

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        => (IGenericRepository<TEntity, TKey>) _repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_context));

        

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
