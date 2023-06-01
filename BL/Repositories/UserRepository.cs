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
        //IEnumerable<BLUser> GetAll();
        //BLUser GetById(int id);
        //BLUser Add(BLUser user);
        //BLUser Update(int id, BLUser user);
        //void Delete(int id);


        //User Add(BLUserRegisterRequest request);
        //void ValidateEmail(BLValidateEmailRequest request);
        //BLTokens JwtTokens(BLJwtTokensRequest request);
        //void ChangePassword(BLChangePasswordRequest request);


        IEnumerable<BLUser> GetAll();
        BLUser GetById(int id);
        BLUser GetByUsername(string username);
        BLUser CreateUser(string username, string firstName, string lastName, string email, string phone, string password, int country);
        void ConfirmEmail(string email, string securityToken);
        BLUser GetConfirmedUser(string username, string password);
        void ChangePassword(string username, string newPassword);

        BLUser UpdateUser(int id, BLUser user);
        void DeleteUser(int id);

        void SoftDeleteUser(int id);

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





        //public IEnumerable<BLUser> GetAll()
        //{
        //    var dbUsers = _dbContext.Users;
        //    var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

        //    return blUsers;


        //}

        //public BLUser GetById(int id)
        //{
        //    var dbUser = _dbContext.Users.FirstOrDefault(s => s.Id == id);
        //    var blUser = _mapper.Map<BLUser>(dbUser);

        //    return blUser;



        //}

        //public BLUser Add(BLUser user)
        //{
        //    var newDbUser = _mapper.Map<User>(user);
        //    newDbUser.Id = 0;
        //    _dbContext.Users.Add(newDbUser);
        //    _dbContext.SaveChanges();

        //    var newBlUser = _mapper.Map<BLUser>(newDbUser);
        //    return newBlUser;
        //}

        //public BLUser Update(int id, BLUser user)
        //{
        //    var dbUser = _dbContext.Users.Find(id);
        //    if (dbUser == null)
        //    {
        //        throw new InvalidOperationException("User not found");
        //    }

        //    _mapper.Map(user, dbUser);
        //    _dbContext.SaveChanges();

        //    var updatedBlUser = _mapper.Map<BLUser>(dbUser);
        //    return updatedBlUser;
        //}

        //public void Delete(int id)
        //{
        //    var dbUser = _dbContext.Users.Find(id);
        //    if (dbUser == null)
        //    {
        //        throw new InvalidOperationException("User not found");
        //    }

        //    _dbContext.Users.Remove(dbUser);
        //    _dbContext.SaveChanges();
        //}



        //public User Add(BLUserRegisterRequest request)
        //{
        //    // Username: Normalize and check if username exists
        //    var normalizedUsername = request.Username.ToLower().Trim();
        //    if (_users.Any(x => x.Username.Equals(normalizedUsername)))
        //        throw new InvalidOperationException("Username already exists");

        //    // Password: Salt and hash password
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        //    string b64Salt = Convert.ToBase64String(salt);

        //    byte[] hash =
        //        KeyDerivation.Pbkdf2(
        //            password: request.Password,
        //            salt: salt,
        //            prf: KeyDerivationPrf.HMACSHA256,
        //            iterationCount: 100000,
        //            numBytesRequested: 256 / 8);
        //    string b64Hash = Convert.ToBase64String(hash);

        //    // SecurityToken: Random security token
        //    byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
        //    string b64SecToken = Convert.ToBase64String(securityToken);

        //    // Id: Next id
        //    int nextId = 1;
        //    if (_users.Any())
        //    {
        //        nextId = _users.Max(x => x.Id) + 1;
        //    }

        //    // New user
        //    var newUser = new User
        //    {
        //        Id = nextId,
        //        Username = request.Username,
        //        Email = request.Email,
        //        Phone = request.Phone,
        //        IsConfirmed = false,
        //        SecurityToken = b64SecToken,
        //        PwdSalt = b64Salt,
        //        PwdHash = b64Hash,

        //    };
        //    _users.Add(newUser);

        //    return newUser;
        //}

        //public void ValidateEmail(BLValidateEmailRequest request)
        //{
        //    var target = _users.FirstOrDefault(x =>
        //        x.Username == request.Username && x.SecurityToken == request.B64SecToken);

        //    if (target == null)
        //        throw new InvalidOperationException("Authentication failed");

        //    target.IsConfirmed = true;
        //}

        //private bool Authenticate(string username, string password)
        //{
        //    var target = _users.Single(x => x.Username == username);

        //    if (!target.IsConfirmed)
        //        return false;

        //    // Get stored salt and hash
        //    byte[] salt = Convert.FromBase64String(target.PwdSalt);
        //    byte[] hash = Convert.FromBase64String(target.PwdHash);

        //    byte[] calcHash =
        //        KeyDerivation.Pbkdf2(
        //            password: password,
        //            salt: salt,
        //            prf: KeyDerivationPrf.HMACSHA256,
        //            iterationCount: 100000,
        //            numBytesRequested: 256 / 8);

        //    return hash.SequenceEqual(calcHash);
        //}

        ////public string GetRole(string username)
        ////{
        ////    var target = _users.Single(x => x.Username == username);
        ////    return target.Role;
        ////}

        //public BLTokens JwtTokens(BLJwtTokensRequest request)
        //{
        //    var isAuthenticated = Authenticate(request.Username, request.Password);

        //    if (!isAuthenticated)
        //        throw new InvalidOperationException("Authentication failed");

        //    // Get secret key bytes
        //    var jwtKey = _configuration["JWT:Key"];
        //    var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);
        //    //var role = GetRole(request.Username);

        //    // Create a token descriptor (represents a token, kind of a "template" for token)
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
        //        {
        //            new System.Security.Claims.Claim(ClaimTypes.Name, request.Username),
        //            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, request.Username),
        //            //new System.Security.Claims.Claim(ClaimTypes.Role, role)
        //        }),
        //        Issuer = _configuration["JWT:Issuer"],
        //        Audience = _configuration["JWT:Audience"],
        //        Expires = DateTime.UtcNow.AddMinutes(10),
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(jwtKeyBytes),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    // Create token using that descriptor, serialize it and return it
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var serializedToken = tokenHandler.WriteToken(token);

        //    return new BLTokens
        //    {
        //        Token = serializedToken
        //    };
        //}

        //public void ChangePassword(BLChangePasswordRequest request)
        //{
        //    var isAuthenticated = Authenticate(request.Username, request.OldPassword);

        //    if (!isAuthenticated)
        //        throw new InvalidOperationException("Authentication failed");

        //    // Salt and hash pwd
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        //    string b64Salt = Convert.ToBase64String(salt);

        //    byte[] hash =
        //        KeyDerivation.Pbkdf2(
        //            password: request.NewPassword,
        //            salt: salt,
        //            prf: KeyDerivationPrf.HMACSHA256,
        //            iterationCount: 100000,
        //            numBytesRequested: 256 / 8);
        //    string b64Hash = Convert.ToBase64String(hash);

        //    // Update user
        //    var target = _users.Single(x => x.Username == request.Username);
        //    target.PwdSalt = b64Salt;
        //    target.PwdHash = b64Hash;
        //}

        //-----------------------ovo gore treba vidjeti, ovo dolje vježbe 12

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

    
            public BLUser CreateUser(string username, string firstName, string lastName, string email,string phone, string password, int country)
        {
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


    }
}


