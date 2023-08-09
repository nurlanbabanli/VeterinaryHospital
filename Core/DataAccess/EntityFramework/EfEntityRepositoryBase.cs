using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEfEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                await context.SaveChangesAsync();

                return addedEntity.Entity;
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> whereExpression = null)
        {
            using (var context = new TContext())
            {
                return await (whereExpression==null
                 ? context.Set<TEntity>().ToListAsync()
                 : context.Set<TEntity>().Where(whereExpression).ToListAsync());
            }
        }

         async Task<List<TEntity>> IEfEntityRepository<TEntity>.GetAllAsync<JoinType>(Expression<Func<TEntity, JoinType>> includeExpression, Expression<Func<TEntity, bool>> whereExpression)
        {
            if (includeExpression==null) throw new ArgumentNullException("EfEntityRepositoryBase->IncludeExpression is  null");

            using (var context = new TContext())
            {
                return await(whereExpression==null
                ? context.Set<TEntity>().Include(includeExpression).ToListAsync()
                : context.Set<TEntity>().Include(includeExpression).Where(whereExpression).ToListAsync());
            }
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().SingleOrDefaultAsync(expression);
            }
        }

        async Task<TEntity> IEfEntityRepository<TEntity>.GetAsync<JoinType>(Expression<Func<TEntity, JoinType>> includeExpression, Expression<Func<TEntity, bool>> expression)
        {
            if (includeExpression==null) throw new ArgumentNullException("EfEntityRepositoryBase->IncludeExpression is  null");

            using (var context = new TContext())
            {
                return await context.Set<TEntity>().Include(includeExpression).SingleOrDefaultAsync(expression);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return updatedEntity.Entity;
            }
        }
    }
}
