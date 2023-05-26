using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<BLTag> GetAll();
        BLTag GetById(int id);
        BLTag Add(BLTag tag);
        BLTag Update(int id, BLTag tag);
        void Delete(int id);
    }

    public class TagRepository : ITagRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public TagRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<BLTag> GetAll()
        {
            var dbTags = _dbContext.Tags;
            var blTags = _mapper.Map<IEnumerable<BLTag>>(dbTags);

            return blTags;
        }

        public BLTag GetById(int id)
        {
            var dbTag = _dbContext.Tags.FirstOrDefault(s => s.Id == id);
            var blTag = _mapper.Map<BLTag>(dbTag);

            return blTag;
        }

        public BLTag Add(BLTag tag)
        {
            var newDbTag = _mapper.Map<Tag>(tag);
            newDbTag.Id = 0;
            _dbContext.Tags.Add(newDbTag);
            _dbContext.SaveChanges();

            var newBlTag = _mapper.Map<BLTag>(newDbTag);
            return newBlTag;
        }

        public BLTag Update(int id, BLTag tag)
        {
            var dbTag = _dbContext.Tags.FirstOrDefault(s => s.Id == id);
            if (dbTag == null)
            {
                throw new InvalidOperationException("Tag not found");
            }

            _mapper.Map(tag, dbTag);
            _dbContext.SaveChanges();

            var updatedBlTag = _mapper.Map<BLTag>(dbTag);
            return updatedBlTag;
        }

        public void Delete(int id)
        {
            var dbTag = _dbContext.Tags.FirstOrDefault(s => s.Id == id);
            if (dbTag == null)
            {
                throw new InvalidOperationException("Tag not found");
            }

            _dbContext.Tags.Remove(dbTag);
            _dbContext.SaveChanges();
        }
    }

}
