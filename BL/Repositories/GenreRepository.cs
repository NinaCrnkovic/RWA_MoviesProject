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


    }
    public class GenreRepository
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
            var dbGenre = _dbContext.Users;
            var blGenre = _mapper.Map<IEnumerable<BLGenre>>(dbGenre);

            return blGenre;
        }
    }
}
