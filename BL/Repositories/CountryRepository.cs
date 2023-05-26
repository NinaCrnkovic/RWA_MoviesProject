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
        BLCountry GetById(int id);
        BLCountry Add(BLCountry country);
        BLCountry Update(int id, BLCountry country);
        void Delete(int id);
    }

    public class CountryRepository : ICountryRepository
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
            var dbCountries = _dbContext.Countries;
            var blCountries = _mapper.Map<IEnumerable<BLCountry>>(dbCountries);

            return blCountries;
        }

        public BLCountry GetById(int id)
        {
            var dbCountry = _dbContext.Countries.FirstOrDefault(s => s.Id == id);
            var blCountry = _mapper.Map<BLCountry>(dbCountry);

            return blCountry;
        }

        public BLCountry Add(BLCountry country)
        {
            var newDbCountry = _mapper.Map<Country>(country);
            newDbCountry.Id = 0;
            _dbContext.Countries.Add(newDbCountry);
            _dbContext.SaveChanges();

            var newBlCountry = _mapper.Map<BLCountry>(newDbCountry);
            return newBlCountry;
        }

        public BLCountry Update(int id, BLCountry country)
        {
            var dbCountry = _dbContext.Countries.FirstOrDefault(s => s.Id == id);
            if (dbCountry == null)
            {
                throw new InvalidOperationException("Country not found");
            }

            _mapper.Map(country, dbCountry);
            _dbContext.SaveChanges();

            var updatedBlCountry = _mapper.Map<BLCountry>(dbCountry);
            return updatedBlCountry;
        }

        public void Delete(int id)
        {
            var dbCountry = _dbContext.Countries.FirstOrDefault(s => s.Id == id);
            if (dbCountry == null)
            {
                throw new InvalidOperationException("Country not found");
            }

            _dbContext.Countries.Remove(dbCountry);
            _dbContext.SaveChanges();
        }
    }

}
