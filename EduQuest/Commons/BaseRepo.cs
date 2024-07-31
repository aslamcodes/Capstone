using Microsoft.EntityFrameworkCore;

namespace EduQuest.Commons
{
    public abstract class BaseRepo<K, T>(EduQuestContext context) : IRepository<K, T> where T : BaseEntity
    {
        public virtual async Task<T> Add(T entity)
        {
            await context.AddAsync(entity);

            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T> Delete(K key)
        {
            var entity = await GetByKey(key);

            context.Set<T>().Remove(entity);

            await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();

        }

        public virtual async Task<T> GetByKey(K key)
        {
            var entity = await context.Set<T>()
                                      .SingleOrDefaultAsync(entity => entity.Id.Equals(key)) ?? throw new EntityNotFoundException($"{typeof(T).Name} with key {key} not found!!!");

            context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            context.Update(entity);

            await context.SaveChangesAsync();

            context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
    }
}
