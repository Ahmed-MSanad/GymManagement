using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        T? GetById(int id);
        IEnumerable<T> GetAll(Func<T, bool>? condition = null);
        void Add(T TEntity); // returns number of the affected rows in the Db from the SaveChanges()
        void Update(T TEntity); // returns number of the affected rows in the Db from the SaveChanges()
        void Delete(T TEntity); // returns number of the affected rows in the Db from the SaveChanges()
    }
}
