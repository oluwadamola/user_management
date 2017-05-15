    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
//using System.Reflection;
using System.Data.Entity.Validation;
using UserManagement.Models.Interface.DataAccess;


namespace UserManagement.Models.DataAccess
{
    public class EntityRepository : IDataRepository
    {
        private DbContext _dbContext;

        public EntityRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = Query<T>(); 
            foreach(var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public void Add<T>(T item) where T : class
        {
            _dbContext.Set<T>().Add(item);
        }

        public void Delete<T>(T item) where T : class
        {
            _dbContext.Set<T>().Remove(item);
        }

        public T GetByID<T>(int id) where T : class
        {
            return _dbContext.Set<T>().Find(id);     
        }

        public void Update<T>(T item) where T : class
        {
            if(_dbContext.Entry(item).State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(item);
            }
            _dbContext.Entry(item).State = EntityState.Modified;
        }


        public IEnumerable<T> Execute<T>(string sprocname, object args) where T : class
        {
            var argProperties = args.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            //Get SQL Parameters Using Reflection
            var parameters = argProperties.Select(PropertyInfo => new System.Data.SqlClient.SqlParameter(string.Format("@{0}", PropertyInfo.Name),
                PropertyInfo.GetValue(args, new object[] { }))).ToList();
            
            //Build Sql query to Execute Query using Parameters
            string queryString = string.Format("{0}", sprocname);
            parameters.ForEach(X => queryString = string.Format("{0} {1},", queryString, X.ParameterName));
            string format = queryString.TrimEnd(',');

            //finally Excute Query
            return _dbContext.Database.SqlQuery<T>(format, parameters.Cast<object>().ToArray());
        }

        public IEnumerable<T> Execute<T>(string sql)
        {
            return _dbContext.Database.SqlQuery<T>(sql);
        }

        public void SaveChange()
        {
             //var operation = new Operation<int>();
            try
            {
                int row = _dbContext.SaveChanges();
            }
            catch(DbEntityValidationException dbex)
            {
                var message = "Entity Validation Failed: ";
                /*foreach (var error in dbex.EntityValidationErrors)
                {
                    foreach (var err in error.ValidationErrors)
                    {
                        
                        message += "\n" + err.PropertyName + "-- " + err.ErrorMessage ;
                    }
                }*/

                message = dbex.EntityValidationErrors
                    .SelectMany(v => v.ValidationErrors)
                    .Select(e => e.ErrorMessage)
                    .Aggregate((ag, e) => ag + " , " + e );
                throw new Exception(message);
                //operation.Succeeded = false;
                //operation.Message = message;
            }
            catch(Exception ex)
            {
                while(ex.InnerException != null)
                    ex = ex.InnerException;
                throw new Exception(ex.Message);
                //operation.Message = ex.Message;
                //operation.Succeeded = false;
            }
        }

        public void Execute(string sql, object args)
        {
            var argProperties = args.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            //Get SQL Parameters Using Reflection
            var parameters = argProperties.Select(PropertyInfo => new System.Data.SqlClient.SqlParameter(string.Format("@{0}", PropertyInfo.Name),
                PropertyInfo.GetValue(args, new object[] { }))).ToList();

            //finally Excute Query
             _dbContext.Database.ExecuteSqlCommand(sql, parameters.Cast<object>().ToArray());
        }

        public void Execute(string sql)
        {
            _dbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}