using Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository.DbData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Repository
{
    // in case we have similar objects that can have the same parent
    // all operations with database link (RepositoryContext) will be set here
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        // AsNoTracking() is recommended when your query is meant for read operations
        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
              RepositoryContext.Set<T>().AsNoTracking() :
              RepositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
              RepositoryContext.Set<T>().Where(expression).AsNoTracking() :
              RepositoryContext.Set<T>()
                .Where(expression);


        // some async method
        public async Task<T> Get<TKey>(Expression<Func<T, bool>> filter = null, string includeProperties = "", bool noTracking = false)
        {
            includeProperties = includeProperties.Trim() ?? string.Empty;
            IQueryable<T> query = RepositoryContext.Set<T>();

            if (noTracking)  
                query.AsNoTracking();
            if (filter != null) 
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {

            IQueryable<T> query = RepositoryContext.Set<T>();

            // generic orderby Func
            //Func<TSource, TKey> orderByFunc = null;
            //if (orderBy != null)
            //  query = query.OrderBy(orderByFunc);
            
            // sample usage
            Func<IQueryable<ObjectA>, IOrderedQueryable<ObjectA>> orderingFunc = (query => query.OrderBy(x => x.Id));

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var obj = query.ToListAsync<T>();

            return await query.ToListAsync<T>();
        }


        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        // tell if changes in database where successfully saved
        public bool SaveChanges() => RepositoryContext.SaveChanges() >= 0;
    }

    public static class Extensions
    {
        // An OrderBy extension method for IQueryable that takes string // special thanks to: neoGeneva
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (string.IsNullOrEmpty(sortExpression))
                throw new ArgumentException("sortExpression is null or empty.", "sortExpression");

            var parts = sortExpression.Split(' ');
            var isDescending = false;
            var propertyName = "";
            var tType = typeof(T);

            if (parts.Length > 0 && parts[0] != "")
            {
                propertyName = parts[0];

                if (parts.Length > 1)
                {
                    isDescending = parts[1].ToLower().Contains("esc");
                }

                PropertyInfo prop = tType.GetProperty(propertyName);

                if (prop == null)
                {
                    throw new ArgumentException(string.Format("No property '{0}' on type '{1}'", propertyName, tType.Name));
                }

                var funcType = typeof(Func<,>)
                    .MakeGenericType(tType, prop.PropertyType);

                var lambdaBuilder = typeof(Expression)
                    .GetMethods()
                    .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                    .MakeGenericMethod(funcType);

                var parameter = Expression.Parameter(tType);
                var propExpress = Expression.Property(parameter, prop);

                var sortLambda = lambdaBuilder
                    .Invoke(null, new object[] { propExpress, new ParameterExpression[] { parameter } });

                var sorter = typeof(Queryable)
                    .GetMethods()
                    .FirstOrDefault(x => x.Name == (isDescending ? "OrderByDescending" : "OrderBy") && x.GetParameters().Length == 2)
                    .MakeGenericMethod(new[] { tType, prop.PropertyType });

                return (IQueryable<T>)sorter
                    .Invoke(null, new object[] { source, sortLambda });
            }

            return source;
        }
    }


    // just an example for passing func to a method
    public class ObjectA { public int Id { get; set; } }
    /*public static class Helper
    {
        public static void Test(Func<IQueryable<ObjectA>, IOrderedQueryable<ObjectA>> param)
        {
            var test = 0;
        }
        static void Usage()
        {
            var student = new List<ObjectA>().AsQueryable(); // just for example
            Helper.Test(m => m.OrderBy(x => x.Id));

            //function variable: it's easy to pass it to the method
            Func<IQueryable<ObjectA>, IOrderedQueryable<ObjectA>> orderingFunc = (query => query.OrderBy(x => x.Id));
        }
    }*/

}
