using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public abstract class TravcomViewModelBase
    {
        protected TravComEntities Context;
    }

    public abstract class TravcomViewModelBase<TEntity> : TravcomViewModelBase
        where TEntity : class, new()
    {
        protected TEntity Entity;

        protected TEntity DatabaseEntity;

        public TravcomViewModelBase()
        {
            Context = new TravComEntities();
        }

        protected void CreateEntity()
        {
            Entity = new TEntity();
        }

        protected void Add(TEntity _entity)
        {
            _entity.GetType().GetProperty("ID").SetValue(_entity, Guid.NewGuid());

            Context.Set<TEntity>().Add(_entity);

            Context.SaveChanges();

            Context.Dispose();
        }

        protected void Update(TEntity _entity)
        {
            Context.Entry(_entity).State = EntityState.Modified;

            Context.SaveChanges();

            Context.Dispose();
        }

        protected TEntity GetEntity(Func<TEntity, Boolean> predicate)
        {
            var returnVal = Context.Set<TEntity>().FirstOrDefault(predicate);

            Context.Dispose();

            return returnVal;
        }

        protected TEntity GetLast(Func<TEntity, Boolean> predicate)
        {
            return Context.Set<TEntity>().LastOrDefault(predicate);
        }

        protected List<TEntity> Find(Func<TEntity, bool> predicate)
        {
            Context = new TravComEntities();

            var returnVal = Context.Set<TEntity>().Where(predicate).ToList();

            Context.Dispose();

            return returnVal;
        }

        protected List<TEntity> Find()
        {
            return Context.Set<TEntity>().ToList();
        }
    }
}
