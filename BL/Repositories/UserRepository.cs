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

    public interface IUserRepository
    {
        IEnumerable<BLUser> GetAll();

        BLUser GetById(int id);

        BLUser Add(BLUser user);


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

        public BLUser GetById(int id)
        {
            var dbUser = _dbContext.Users.Find(id);
            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

        public BLUser Add(BLUser user)
        {
            var newUser = _mapper.Map<User>(user);
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            var blUser = _mapper.Map<BLUser>(newUser);
            return blUser;
        }

        public BLUser Update(int id, BLUser user)
        {
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            _mapper.Map(user, dbUser);
            _dbContext.SaveChanges();

            var blUser = _mapper.Map<BLUser>(dbUser);
            return blUser;
        }

        public void Delete(int id)
        {
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            _dbContext.Users.Remove(dbUser);
            _dbContext.SaveChanges();
        }

    }
}
