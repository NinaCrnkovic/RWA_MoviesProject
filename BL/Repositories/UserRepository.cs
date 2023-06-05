using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{

    public interface IUserRepository
    {
        


        IEnumerable<BLUser> GetAll();
        BLUser GetById(int id);
        BLUser GetByUsername(string username);
        BLUser CreateUserMVC(string username, string firstName, string lastName, string email, string phone, string password, int country);
        void ConfirmEmail(string email, string securityToken);
        BLUser GetConfirmedUser(string username, string password);
        void ChangePassword(string username, string newPassword);

        BLUser UpdateUser(int id, BLUser user);
        void DeleteUser(int id);

        void SoftDeleteUser(int id);

        string GenerateJwtToken(string email);

        BLUser GetAuthenticatedUser(string username, string password);

    }

    public class UserRepository : IUserRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;


        private readonly List<User> _users = new List<User>();
        private readonly IConfiguration _configuration;

        public UserRepository(RwaMoviesContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IEnumerable<BLUser> GetAll()
        {
            // First retrieve collection from database
            var dbUsers = _dbContext.Users;

            // Then Map it to BL model collection using IMapper
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;
        }

        public BLUser GetById(int id)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(s => s.Id == id);
            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

        public BLUser GetByUsername(string username)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Email == username);
            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

    
        public BLUser CreateUserMVC(string username, string firstName, string lastName, string email,string phone, string password, int country)
        {
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new Exception("A user with the same email already exists.");
            }
            (var salt, var b64Salt) = GenerateSalt();
            var b64Hash = CreateHash(password, salt);
            var b64SecToken = GenerateSecurityToken();

            // Create BLUser object
            var dbUser = new User()
            {
                CreatedAt = DateTime.UtcNow,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                PwdHash = b64Hash,
                PwdSalt = b64Salt,
                SecurityToken = b64SecToken,
                CountryOfResidenceId = country
                
            };
            _dbContext.Users.Add(dbUser);

            _dbContext.SaveChanges();

            var blUser = _mapper.Map<BLUser>(dbUser);
            return blUser;
        }

        public void ConfirmEmail(string email, string securityToken)
        {
            var userToConfirm = _dbContext.Users.FirstOrDefault(x =>
                x.Email == email &&
                x.SecurityToken == securityToken);

            userToConfirm.IsConfirmed = true;

            _dbContext.SaveChanges();
        }

        public BLUser GetConfirmedUser(string username, string password)
        {
            //var dbUser = _dbContext.Users.FirstOrDefault(x =>
            //    x.Username == username &&
            //    x.IsConfirmed == true);
            //zbog is confirmed
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (dbUser == null)
            {
                throw new InvalidOperationException("Wrong username or password");
            }

            var salt = Convert.FromBase64String(dbUser.PwdSalt);
            var b64Hash = CreateHash(password, salt);

            if (dbUser.PwdHash != b64Hash)
            {
                throw new InvalidOperationException("Wrong username or password");
            }

            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

        public void ChangePassword(string username, string newPassword)
        {
            User userToChangePassword = _dbContext.Users.FirstOrDefault(x =>
                x.Username == username &&
                !x.DeletedAt.HasValue);

            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(newPassword, salt);

            userToChangePassword.PwdHash = b64Hash;
            userToChangePassword.PwdSalt = b64Salt;

            _dbContext.SaveChanges();
        }

        private static (byte[], string) GenerateSalt()
        {
            // Generate salt
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var b64Salt = Convert.ToBase64String(salt);

            return (salt, b64Salt);
        }

        private static string CreateHash(string password, byte[] salt)
        {
            // Create hash from password and salt
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }

        private static string GenerateSecurityToken()
        {
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            return b64SecToken;
        }

        public BLUser UpdateUser(int id, BLUser user)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (dbUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;
            dbUser.CountryOfResidenceId = user.CountryOfResidenceId;

            _dbContext.SaveChanges();

            var blUser = _mapper.Map<BLUser>(dbUser);
            return blUser;
        }

        public void DeleteUser(int id)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (dbUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            _dbContext.Users.Remove(dbUser);
            _dbContext.SaveChanges();

        }

        public void SoftDeleteUser(int id)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (dbUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            dbUser.DeletedAt = DateTime.Now;
            _dbContext.SaveChanges();

            
        }

        public string GenerateJwtToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpirationDays"])),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }




        public BLUser GetAuthenticatedUser(string username, string password)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (dbUser == null)
            {
                throw new InvalidOperationException("Wrong username or password");
            }

            var salt = Convert.FromBase64String(dbUser.PwdSalt);
            var b64Hash = CreateHash(password, salt);

            if (dbUser.PwdHash != b64Hash)
            {
                throw new InvalidOperationException("Wrong username or password");
            }

            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }


    }
}


