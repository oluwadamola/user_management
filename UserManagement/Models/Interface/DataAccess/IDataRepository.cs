using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserManagement.Models.Interface.DataAccess
{
    public interface IDataRepository
    {
        IQueryable<T> Query<T>() where T : class;
        IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;

        void Add<T>(T item) where T : class;
        void Delete<T>(T item) where T : class;
        T GetByID<T>(int id) where T : class;
        void Update<T>(T item) where T : class;
        IEnumerable<T> Execute<T>(string  sprocname, object args) where T : class;
        IEnumerable<T> Execute<T>(string sql);
        void SaveChange();
        //void Execute<T>(string sql, object args) where T : class;
        //void Execute<T>(string sql) where T : class;
        
    }
}
