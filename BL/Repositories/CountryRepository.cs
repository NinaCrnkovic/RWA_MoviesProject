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

    public interface ICountryRepository
    {
        IEnumerable<BLCountry> GetAll();


    }

    public class CountryRepository
    {
        private readonly RwaMoviesContext _dbContext;
         private readonly IMapper _mapper;
        public CountryRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
              _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<BLCountry> GetAll()
        {
        var dbCountry = _dbContext.Users;
        var blCountry = _mapper.Map<IEnumerable<BLCountry>>(dbCountry);

        return blCountry;

        }

    }
}
