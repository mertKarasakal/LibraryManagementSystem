using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LibraryManagementSystem.WebUI.DataAccess.Abstract;
using LibraryManagementSystem.WebUI.Entity.Abstract;
using LibraryManagementSystem.WebUI.Utilities.Security;

namespace LibraryManagementSystem.WebUI.DataAccess.Concrete.EntityFramework {
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new() {
        public void Add(TEntity entity) {
            using (var context = new TContext()) {
                entity.UpdateDate = DateTime.Now;
                entity.UpdatingUserCode = UserRoleProvider.UserId.ToString();
                entity.UpdatingTranCode = "ADD";
                entity.RecordStatus = true;
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;

                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity) {
            using (var context = new TContext()) {
                entity.RecordStatus = false;
                entity.UpdatingTranCode = "DELETE";
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter) {
            using (var context = new TContext()) {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null) {
            using (var context = new TContext()) {
                return filter == null ?
                    context.Set<TEntity>().ToList() :
                    context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity) {
            using (var context = new TContext()) {
                entity.UpdateDate = DateTime.Now;
                entity.UpdatingUserCode = UserRoleProvider.UserId.ToString();
                entity.UpdatingTranCode = "UPDATE";
                entity.RecordStatus = true;
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}