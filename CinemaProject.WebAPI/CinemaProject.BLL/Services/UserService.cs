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

        public User[] GetAll()
        {
            IEnumerable<UserEntity> usersEntity = _unitOfWork.UsersRepository.GetWithInclude(user => user.Tickets);

            return usersEntity
                .Select(user => user.ToModel())
                .ToArray();
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
            userEntity.Email = user.Email;
            userEntity.Login = user.Login;
            userEntity.Password = user.Password;
            userEntity.IsAdmin = user.IsAdmin;

            await _unitOfWork.UsersRepository.UpdateAsync(user.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            IEnumerable<UserEntity> usersEntity = await _unitOfWork.UsersRepository.GetAllAsync();

            UserEntity authenticateUserEntity = usersEntity.SingleOrDefault(u => u.Login == model.Login && BC.Verify(model.Password, u.Password));

            if (authenticateUserEntity == null)
            {
                return null;
            }

            UserEntity user = await _unitOfWork.UsersRepository.GetAsync(authenticateUserEntity.Id);

            string token = generateJwtToken(user);
            RefreshTokenEntity refreshToken = generateRefreshToken(ipAddress);

            refreshToken.UserId = user.Id;
            await _unitOfWork.UsersRepository.InsertRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveAsync();

            user.RefreshTokens.Add(refreshToken);
            await _unitOfWork.UsersRepository.UpdateAsync(authenticateUserEntity.Id);
            await _unitOfWork.SaveAsync();

            return new AuthenticateResponse(token, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> Register(RegisterRequest model, string ipAddress)
        {
            IEnumerable<UserEntity> users = await _unitOfWork.UsersRepository.GetAllAsync();

            if (users.Any(user => user.Email == model.Email))
            {
                return null;
            }

            UserEntity newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Login = model.Login,
                Password = BC.HashPassword(model.Password),
                IsAdmin = false
            };

            string token = generateJwtToken(newUser);
            RefreshTokenEntity refreshToken = generateRefreshToken(ipAddress);

            refreshToken.UserId = newUser.Id;
            await _unitOfWork.UsersRepository.InsertRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveAsync();

            newUser.RefreshTokens.Add(refreshToken);

            await _unitOfWork.UsersRepository.InsertAsync(newUser);
            await _unitOfWork.SaveAsync();

            return new AuthenticateResponse(token, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            IEnumerable<UserEntity> users = await _unitOfWork.UsersRepository.GetAllAsync();

            UserEntity authenticatedUser = users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (authenticatedUser == null)
            {
                return null;
            }

            RefreshTokenEntity refreshToken = authenticatedUser.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return null;
            }

            RefreshTokenEntity newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            authenticatedUser.RefreshTokens.Add(newRefreshToken);
            await _unitOfWork.UsersRepository.UpdateAsync(authenticatedUser.Id);
            await _unitOfWork.SaveAsync();

            var jwtToken = generateJwtToken(authenticatedUser);

            return new AuthenticateResponse(jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            IEnumerable<UserEntity> users = await _unitOfWork.UsersRepository.GetAllAsync();

            UserEntity authenticatedUser = users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (authenticatedUser == null)
            {
                return false;
            }

            RefreshTokenEntity refreshToken = authenticatedUser.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await _unitOfWork.UsersRepository.UpdateAsync(authenticatedUser.Id);
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
                        new Claim("id", user.Id.ToString()),
                        new Claim("login", user.Login),
                        new Claim("password", user.Password)
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(1),
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