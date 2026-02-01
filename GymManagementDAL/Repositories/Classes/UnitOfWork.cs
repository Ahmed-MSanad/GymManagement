using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext gymDbContext;
        private readonly Dictionary<string, object> Repositories;
        public ISessionRepository SessionRepository { get; set; }

        public UnitOfWork(GymDbContext _gymDbContext, ISessionRepository _SessionRepository)
        {
            gymDbContext = _gymDbContext;
            SessionRepository = _SessionRepository;
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            string key = typeof(T).Name;

            if (Repositories.TryGetValue(key, out object? value))
                return (IGenericRepository<T>)value;

            var newRepo = new GenericRepository<T>(gymDbContext);
            Repositories.Add(key, newRepo);

            return newRepo;
        }
        public int SaveChanges() => gymDbContext.SaveChanges();
    }
}