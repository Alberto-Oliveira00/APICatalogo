using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public interface IRepository<T>
{
    //Cuidado para não violar o principio ISP (Interface Segregation Principle)
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
