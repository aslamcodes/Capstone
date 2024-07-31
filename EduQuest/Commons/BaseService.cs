
using AutoMapper;

namespace EduQuest.Commons
{
    public abstract class BaseService<T, DTO>(IRepository<int, T> repo, IMapper mapper) : IBaseService<T, DTO> where T : class
    {
        public async Task<DTO> GetById(int id)
        {
            try
            {
                var entity = await repo.GetByKey(id);
                return mapper.Map<DTO>(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO>> GetAll()
        {
            var entities = await repo.GetAll();
            return entities.ConvertAll(input => mapper.Map<DTO>(input)).ToList();
        }

        public virtual async Task<DTO> Add(DTO entity)
        {
            var res = await repo.Add(mapper.Map<T>(entity));

            return mapper.Map<DTO>(res);
        }

        public virtual async Task<DTO> Update(DTO updateEntity)
        {
            try
            {
                var entity = await repo.Update(mapper.Map<T>(updateEntity));
                return mapper.Map<DTO>(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO> DeleteById(int id)
        {
            try
            {
                var deletedEntity = await repo.Delete(id);
                return mapper.Map<DTO>(deletedEntity);
            }
            catch
            {
                throw;
            }
        }
    }
}