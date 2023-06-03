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
        int GetTotalCount();
        IEnumerable<BLGenre> GetPagedData(int page, int size, string orderBy, string direction);

        IEnumerable<BLGenre> GetFilteredData(string term);
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
            var dbGenre = _dbContext.Genres.FirstOrDefault(s => s.Id == id);
            var blGenre = _mapper.Map<BLGenre>(dbGenre);

            return blGenre;
        }
        public int GetTotalCount() => _dbContext.Genres.Count();
        public IEnumerable<BLGenre> GetPagedData(int page, int size, string orderBy, string direction)
        {
            IEnumerable<Genre> dbGenres = _dbContext.Genres.AsEnumerable();

            // Ordering
            if (string.Compare(orderBy, "id", true) == 0)
            {
                dbGenres = dbGenres.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "name", true) == 0)
            {
                dbGenres = dbGenres.OrderBy(x => x.Name);
            }
            else
            {
                // default: order by Id
                dbGenres = dbGenres.OrderBy(x => x.Id);
            }

            // For descending order we just reverse it
            if (string.Compare(direction, "desc", true) == 0)
            {
                dbGenres = dbGenres.Reverse();
            }

            // Now we can page the correctly ordered items
            dbGenres = dbGenres.Skip(page * size).Take(size);

            var blGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);

            return blGenres;
        }

        public IEnumerable<BLGenre> GetFilteredData(string term)
        {
            // It seems more flexible to check both name or description for the search term
            var dbGenres = _dbContext.Genres.Where(x =>
                x.Name.Contains(term) ||
                x.Description.Contains(term));

            var blGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);

            return blGenres;
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
            var dbGenre = _dbContext.Genres.FirstOrDefault(s => s.Id == id);
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
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var dbGenre = _dbContext.Genres.FirstOrDefault(s => s.Id == id);
                    if (dbGenre == null)
                    {
                        throw new InvalidOperationException("Genre not found");
                    }

                    // Retrieve all videos associated with the genre
                    var videos = _dbContext.Videos.Where(v => v.GenreId == id);

                    // Remove the videos associated with the genre
                    _dbContext.Videos.RemoveRange(videos);

                    _dbContext.SaveChanges();
                    // Remove the genre
                    _dbContext.Genres.Remove(dbGenre);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // Rethrow the exception to handle it at a higher level
                }
            }
        }
    }
}

