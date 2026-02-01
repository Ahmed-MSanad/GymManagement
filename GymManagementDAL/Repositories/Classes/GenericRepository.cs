using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly GymDbContext context;
        public GenericRepository(GymDbContext _context)
        {
            context = _context;
        }
        public void Add(T TEntity)
        {
            context.Set<T>().Add(TEntity);
        }

        public T? GetById(int id) => context.Set<T>().Find(id);

        public IEnumerable<T> GetAll(Func<T, bool>? condition = null)
        {
            if(condition is not null) return context.Set<T>().AsNoTracking().Where(condition).ToList();
            else return context.Set<T>().AsNoTracking().ToList();
        }

        public void Delete(T TEntity)
        {
            context.Set<T>().Remove(TEntity);
        }

        public void Update(T TEntity)
        {
            context.Set<T>().Update(TEntity);
        }
    }
}
