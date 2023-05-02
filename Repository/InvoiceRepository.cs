using InvMang.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using InvMang.Data;
using Microsoft.EntityFrameworkCore;

namespace InvMang.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }

    public interface IUnitOfWork : IDisposable
    {
        IRepository<Invoice> InvoiceRepository { get; }
        void Save();
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly applicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(applicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly applicationDbContext _context;
        private bool _disposed;

        private IRepository<Invoice> _invoiceRepository;

        public UnitOfWork(applicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Invoice> InvoiceRepository
        {
            get
            {
                if (_invoiceRepository == null)
                {
                    _invoiceRepository = new Repository<Invoice>(_context);
                }
                return _invoiceRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
