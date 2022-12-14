using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LibraryManagementSystem.WebUI.Entity.Abstract;

namespace LibraryManagementSystem.WebUI.DataAccess.Abstract {
    public interface IEntityRepository<T> where T : class, IEntity, new() {
        T Get(Expression<Func<T, bool>> filter);
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}