using MLM.API.DB;
using MLM.API.DB.Specifications;
using MLM.API.Models;
using MLM.Exception;
using MLM.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.API
{
    public class BaseHandler<T> where T : MLMBaseEntity
    {
        public IRepository<T> EntityRepository { get; set; }
        public ILog Logger { get; set; }
        public IExceptionHandler ExceptionHandler { get; set; }

        public BaseHandler()
        {
            Logger = new Logger(this.GetType());
            ExceptionHandler = new BaseExceptionHandler(this.GetType());
        }

        public virtual T Get(Int64 id)
        {
            return EntityRepository.GetById(id);
        }

        public virtual IEnumerable<T> Get(IEnumerable<Int64> ids)
        {
            return EntityRepository.GetMany(ids);
        }
        
        public virtual IQueryable<T> Get()
        {
            return EntityRepository.GetAll();
        }

        public virtual IEnumerable<T> GetMany(ISpecification<T> specification)
        {
            return EntityRepository.GetMany(specification).ToList();
        }
                
        public virtual T Post(T entity)
        {
            T updated = null;
            if (entity.Id == 0)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                updated = EntityRepository.Add(entity);
            }
            else
            {
                entity.UpdateDate = DateTime.UtcNow;
                updated = EntityRepository.Update(entity);
            }
            try
            {
                //UnitOfWork.Commit();
            }
            catch (DuplicateKeyException ex)
            {
                return HandleDuplicateKeyException(ex, entity);
            }

            return updated;
        }

        public virtual IEnumerable<T> Post(IEnumerable<T> entities)
        {
            var updates = new List<T>();
            foreach (var entity in entities)
            {
                T updated = null;
                if (entity.Id == 0)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    updated = EntityRepository.Add(entity);
                }
                else
                {
                    entity.UpdateDate = DateTime.UtcNow;
                    updated = EntityRepository.Update(entity);
                }
                updates.Add(updated);
            }

            //UnitOfWork.Commit();
            return updates;
        }

        /// <summary>
        /// Method to handle duplicate key exception. Child handlers can override this method to add their own business logic if required.
        /// If this method is not overriden then base handler logs the exception and throw it again.
        /// </summary>
        /// <param name="ex">Duplicate Key Exception</param>
        /// <param name="entity">Record for which this exception was thrown.</param>
        /// <returns></returns>
        public virtual T HandleDuplicateKeyException(DuplicateKeyException ex,  T entity)
        {
            Logger.LogError("Duplicate Key Error while inserting new record", ex);
            throw ex;
        }
    }
}
