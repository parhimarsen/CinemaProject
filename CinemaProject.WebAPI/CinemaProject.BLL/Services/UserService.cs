using CinemaProject.BLL.Helpers;
using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace CinemaProject.BLL.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public UserService(UnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public IQueryable<User> GetAll()
        {
            return _unitOfWork.UsersRepository
                .GetWithInclude(user => user.Tickets)
                .Select(user => new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    Login = user.Login,
                    Password = user.Password,
                    IsAdmin = user.IsAdmin,
                    Tickets = user.Tickets.ToModel()
                });
        }

        public User Get(Guid id)
        {
            UserEntity userEntity = _unitOfWork.UsersRepository
                .GetWithInclude(user => user.Tickets)
                .FirstOrDefault(user => user.Id == id);

            if (userEntity == null)
            {
                return null;
            }

            return userEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.UsersRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(user.Id))
            {
                return;
            }

            UserEntity userEntity = await _unitOfWork.UsersRepository.GetAsync(user.Id);

            userEntity.Id = user.Id;
            userEntity.Email = user.Email ?? userEntity.Email;
            userEntity.Login = user.Login ?? userEntity.Login;
            userEntity.Password = user.Password ?? userEntity.Password;
            userEntity.IsAdmin = user.IsAdmin;

            await _unitOfWork.UsersRepository.UpdateAsync(userEntity.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            IEnumerable<UserEntity> usersEntity = _unitOfWork.UsersRepository.GetAll();

            UserEntity authenticateUserEntity = usersEntity
                .SingleOrDefault(u => u.Login == model.Login && BC.Verify(model.Password, u.Password));

            if (authenticateUserEntity == null)
            {
                return null;
            }

            UserEntity user = await _unitOfWork.UsersRepository
                .GetAsync(authenticateUserEntity.Id);

            //if (user.RefreshTokens.Count != 0)
            //{
            //    user.RefreshTokens.Remove(user.RefreshTokens.FirstOrDefault());
            //    await _unitOfWork.SaveAsync();
            //}

            string token = generateJwtToken(user);
            RefreshTokenEntity refreshToken = generateRefreshToken(ipAddress);

            refreshToken.UserId = user.Id;
            await _unitOfWork.UsersRepository.InsertRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveAsync();

            user.RefreshTokens.Add(refreshToken);
            await _unitOfWork.SaveAsync();

            return new AuthenticateResponse(token, refreshToken.Token);
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest model, string ipAddress)
        {
            IQueryable<UserEntity> userQuery = _unitOfWork.UsersRepository.GetAll();

            if (userQuery.Any(user => user.Email == model.Email))
            {
                return new RegistrationResponse(null, null, "This E-mail already exist");
            }

            UserEntity newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Login = model.Login,
                Password = BC.HashPassword(model.Password),
                IsAdmin = false,
                RefreshTokens = new List<RefreshTokenEntity>()
            };

            string token = generateJwtToken(newUser);
            RefreshTokenEntity refreshToken = generateRefreshToken(ipAddress);

            refreshToken.UserId = newUser.Id;
            newUser.RefreshTokens.Add(refreshToken);

            await _unitOfWork.UsersRepository.InsertAsync(newUser);
            await _unitOfWork.SaveAsync();

            return new RegistrationResponse(token, refreshToken.Token, "Registration is successful");
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            UserEntity authenticatedUser = _unitOfWork.UsersRepository
                .GetWithInclude(user => user.RefreshTokens)
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (authenticatedUser == null)
            {
                return null;
            }

            RefreshTokenEntity refreshToken = authenticatedUser.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return null;
            }

            var jwtToken = generateJwtToken(authenticatedUser);

            return new AuthenticateResponse(jwtToken, refreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token)
        {
            RefreshTokenEntity refreshToken = _unitOfWork.RefreshTokensRepository
                .GetAll()
                .FirstOrDefault(t => t.Token == token);

            if (refreshToken == null)
            {
                return false;
            }

            await _unitOfWork.RefreshTokensRepository.RemoveAsync(refreshToken.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }

        private string generateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("login", user.Login),
                        new Claim("isAdmin", user.IsAdmin.ToString(), ClaimValueTypes.Boolean)
                    }
                ),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokenEntity generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenEntity
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}