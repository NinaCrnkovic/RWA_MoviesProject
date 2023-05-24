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
        BLUser Add(BLUser user);
        BLUser Update(int id, BLUser user);
        void Delete(int id);


        User Add(UserRegisterRequest request);
        void ValidateEmail(ValidateEmailRequest request);
        Tokens JwtTokens(JwtTokensRequest request);
        void ChangePassword(ChangePasswordRequest request);
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
            var newDbUser = _mapper.Map<User>(user);
            newDbUser.Id = 0;
            _dbContext.Users.Add(newDbUser);
            _dbContext.SaveChanges();

            var newBlUser = _mapper.Map<BLUser>(newDbUser);
            return newBlUser;
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

            var updatedBlUser = _mapper.Map<BLUser>(dbUser);
            return updatedBlUser;
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



        public User Add(UserRegisterRequest request)
        {
            // Username: Normalize and check if username exists
            var normalizedUsername = request.Username.ToLower().Trim();
            if (_users.Any(x => x.Username.Equals(normalizedUsername)))
                throw new InvalidOperationException("Username already exists");

            // Password: Salt and hash password
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            // SecurityToken: Random security token
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            // Id: Next id
            int nextId = 1;
            if (_users.Any())
            {
                nextId = _users.Max(x => x.Id) + 1;
            }

            // New user
            var newUser = new User
            {
                Id = nextId,
                Username = request.Username,
                Email = request.Email,
                Phone = request.Phone,
                IsConfirmed = false,
                SecurityToken = b64SecToken,
                PwdSalt = b64Salt,
                PwdHash = b64Hash,
             
            };
            _users.Add(newUser);

            return newUser;
        }

        public void ValidateEmail(ValidateEmailRequest request)
        {
            var target = _users.FirstOrDefault(x =>
                x.Username == request.Username && x.SecurityToken == request.B64SecToken);

            if (target == null)
                throw new InvalidOperationException("Authentication failed");

            target.IsConfirmed = true;
        }

        private bool Authenticate(string username, string password)
        {
            var target = _users.Single(x => x.Username == username);

            if (!target.IsConfirmed)
                return false;

            // Get stored salt and hash
            byte[] salt = Convert.FromBase64String(target.PwdSalt);
            byte[] hash = Convert.FromBase64String(target.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
        }

        //public string GetRole(string username)
        //{
        //    var target = _users.Single(x => x.Username == username);
        //    return target.Role;
        //}

        public Tokens JwtTokens(JwtTokensRequest request)
        {
            var isAuthenticated = Authenticate(request.Username, request.Password);

            if (!isAuthenticated)
                throw new InvalidOperationException("Authentication failed");

            // Get secret key bytes
            var jwtKey = _configuration["JWT:Key"];
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);
            //var role = GetRole(request.Username);

            // Create a token descriptor (represents a token, kind of a "template" for token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.Name, request.Username),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    //new System.Security.Claims.Claim(ClaimTypes.Role, role)
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token using that descriptor, serialize it and return it
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return new Tokens
            {
                Token = serializedToken
            };
        }

        public void ChangePassword(ChangePasswordRequest request)
        {
            var isAuthenticated = Authenticate(request.Username, request.OldPassword);

            if (!isAuthenticated)
                throw new InvalidOperationException("Authentication failed");

            // Salt and hash pwd
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.NewPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            // Update user
            var target = _users.Single(x => x.Username == request.Username);
            target.PwdSalt = b64Salt;
            target.PwdHash = b64Hash;
        }

    }
}


