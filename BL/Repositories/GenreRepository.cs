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
    public interface IGenreRepository
    {
        IEnumerable<BLGenre> GetAll();
        BLGenre GetById(int id);
        BLGenre Add(BLGenre genre);
        BLGenre Update(int id, BLGenre genre);
        void Delete(int id);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public GenreRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<BLGenre> GetAll()
        {
            var dbGenres = _dbContext.Genres;
            var blGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);

            return blGenres;
        }

        public BLGenre GetById(int id)
        {
            var dbGenre = _dbContext.Genres.Find(id);
            var blGenre = _mapper.Map<BLGenre>(dbGenre);

            return blGenre;
        }

        public BLGenre Add(BLGenre genre)
        {
            var newDbGenre = _mapper.Map<Genre>(genre);
            newDbGenre.Id = 0;
            _dbContext.Genres.Add(newDbGenre);
            _dbContext.SaveChanges();

            var newBlGenre = _mapper.Map<BLGenre>(newDbGenre);
            return newBlGenre;
        }

        public BLGenre Update(int id, BLGenre genre)
        {
            var dbGenre = _dbContext.Genres.Find(id);
            if (dbGenre == null)
            {
                throw new InvalidOperationException("Genre not found");
            }

            _mapper.Map(genre, dbGenre);
            _dbContext.SaveChanges();

            var updatedBlGenre = _mapper.Map<BLGenre>(dbGenre);
            return updatedBlGenre;
        }

        public void Delete(int id)
        {
            var dbGenre = _dbContext.Genres.Find(id);
            if (dbGenre == null)
            {
                throw new InvalidOperationException("Genre not found");
            }

            _dbContext.Genres.Remove(dbGenre);
            _dbContext.SaveChanges();
        }
    }
}

