using AutoMapper;
using BL.BLModels;
using DAL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{

    public interface IUserRepository
    {
        IEnumerable<BLUser> GetAll();
     

    }
    public class UserRepository : IUserRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;
        public UserRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<BLUser> GetAll()
        {
            var dbUsers = _dbContext.Users;
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;

        }

    }
}
