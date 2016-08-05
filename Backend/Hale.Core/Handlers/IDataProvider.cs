using DapperExtensions;
using System.Linq;

namespace Hale_Core.Handlers
{
    internal interface IDataProvider
    {
        // All implementing t he interface need to support basic CRUD operations both w and w/o filters.
        void Create<T>(T model) where T : class;

        T Get<T>(int id) where T : class;
        IQueryable<T> List<T>() where T : class;
        IQueryable<T> List<T>(IPredicate predicates) where T : class;
        void Update<T>(T model) where T : class;
        void Delete<T>(T model) where T : class;
    }
}