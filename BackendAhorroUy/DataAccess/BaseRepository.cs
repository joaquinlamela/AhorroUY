using Castle.Core.Internal;
using DataAccessInterface;
using Microsoft.EntityFrameworkCore;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public BaseRepository(DbContext aContext)
        {
            Context = aContext;
        }

        public void Add(T entity)
        {
            if (Context.Database.CanConnect())
            {
                Context.Set<T>().Add(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase); 
            }    
        }

        public void Remove(T entity)
        {
            if (Context.Database.CanConnect()) { 
                Context.Set<T>().Remove(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public void Update(T entity)
        {
            if (Context.Database.CanConnect())
            {
                Context.Set<T>().Update(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public T Get(Guid id)
        {
                if (Context.Database.CanConnect())
                {
                    T entityObteined = Context.Set<T>().Find(id);
                    if (entityObteined == null)
                    {
                        throw new ClientException(RepositoryMessagesException.ErrorGetElementById);
                    }
                    return entityObteined;
                }
                else
                {
                    throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
                }
        }

        public IEnumerable<T> GetAll()
        {
            if (Context.Database.CanConnect())
            {
                List<T> listOfEntities = Context.Set<T>().ToList();
                if (listOfEntities.IsNullOrEmpty())
                {
                    throw new ClientException(RepositoryMessagesException.ErrorGetAllElements);
                }
                return listOfEntities;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }
    }
}
