using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Utils;
using Z.EntityFramework.Plus;

namespace BELibrary.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly CinemaOnlineDbContext Context;

        public Repository(CinemaOnlineDbContext context)
        {
            Context = context;
        }

        public TEntity Get(object id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var entities = Context.Set<TEntity>().ToList();
            var result = new List<TEntity>();
            foreach (var item in entities)
            {
                if ((bool)item.GetType().GetProperty("IsDelete").GetValue(item) != true)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            entity.GetType().GetProperty("IsDelete").SetValue(entity, false);
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Put(TEntity entity, object id)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            TEntity exist = Context.Set<TEntity>().Find(id);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
        {
            IDbSet<TEntity> dbSet = Context.Set<TEntity>();

            IEnumerable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.Include(include);
            }

            return query ?? dbSet;
        }

        public IEnumerable<TEntity> IncludeFilter(params Expression<Func<TEntity, object>>[] includes)
        {
            IDbSet<TEntity> dbSet = Context.Set<TEntity>();

            IEnumerable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.IncludeFilter(include);
            }

            return query ?? dbSet;
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Where(filter);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(query, parameters);
        }

        public void Del(object id, Guid userId)
        {
            var entity = Context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            entity.GetType().GetProperty("IsDelete").SetValue(entity, true);
            entity.GetType().GetProperty("DeletetionTime").SetValue(entity,DateTime.Now);
            entity.GetType().GetProperty("DeleterId").SetValue(entity, userId);
            Context.SaveChanges();
        }

        public void Update(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            var exist = Context.Set<TEntity>().Find(entity.GetType().GetProperty("Id").GetValue(entity));
            if (exist != null)
            {
                foreach (var prop in entity.GetType().GetProperties())
                {
                    var propValue = entity.GetType().GetProperty(prop.Name).GetValue(entity);
                    if (!prop.GetType().Equals(typeof(List<>)))
                    {
                        try
                        {
                            if (exist.GetType().GetProperty(prop.Name) != null)
                            {
                                exist.GetType().GetProperty(prop.Name).SetValue(exist, propValue);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                        }
                    }
                }
                Context.Entry(exist).CurrentValues.SetValues(exist);
            }
        }

        public void Insert(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            var newEntity = new TEntity();
            newEntity.GetType().GetProperty("IsDelete").SetValue(newEntity, false);
            foreach (var prop in entity.GetType().GetProperties())
            {
                var propValue = entity.GetType().GetProperty(prop.Name).GetValue(entity);
                if (propValue != null)
                {
                    try
                    {
                        if (newEntity.GetType().GetProperty(prop.Name)!=null)
                        {

                            newEntity.GetType().GetProperty(prop.Name).SetValue(newEntity, propValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
            }
            Context.Set<TEntity>().Add(newEntity);
        }

        public Guid InsertReturnId(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            var newEntity = new TEntity();
            var entities = Context.Set<TEntity>().ToList();
            Guid newId = Guid.NewGuid();
            if (entities.Any())
            {
                var idList = entities.Select(e => e.GetType().GetProperty("Id").GetValue(e)).ToList();
                while (idList.Contains(newId))
                {
                    newId = Guid.NewGuid();
                }
            }
            
            newEntity.GetType().GetProperty("Id").SetValue(newEntity, newId);
            newEntity.GetType().GetProperty("IsDelete").SetValue(newEntity, false);
            foreach (var prop in entity.GetType().GetProperties())
            {
                var propValue = entity.GetType().GetProperty(prop.Name).GetValue(entity);
                if (propValue != null)
                {
                    try
                    {
                        newEntity.GetType().GetProperty(prop.Name).SetValue(newEntity, propValue);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
            }
            Context.Set<TEntity>().Add(newEntity);
            return newId;
        }
    }
}