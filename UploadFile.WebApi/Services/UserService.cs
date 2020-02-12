using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Jwt;
using UploadFile.WebApi.Models;
using UploadFile.WebApi.Repositories.Interfaces;
using UploadFile.WebApi.Services.Interfaces;

namespace UploadFile.WebApi.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _repo;
        private readonly TokenConfigurations _tokenConfigurations;

        public UserService(IHttpContextAccessor accessor, IUserRepository repo,
            TokenConfigurations tokenConfigurations)
        :base(accessor)
        {
            _tokenConfigurations = tokenConfigurations;
            _repo = repo;
        }

        public async Task<object> Auth(LoginDTO login)
        {
            if (login.Email == null || login.Password == null)
            {
                throw new Exception ("Usuario nulo");
            }

            var user = await _repo.GetUserByEmail(login.Email, login.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuario Não existe");
            }

            return TokenGenerate(user);
        }

        public async Task Delete(long userId)
        {
            await _repo.UserDelete(userId);
        }

        public async Task<User> Get(long userId)
        {
            var user = await _repo.Get(userId);

            user.Password = null;

            return user;
        }

        public async Task<List<User>> GetAll()
        {
            var users = await _repo.GetAll();

            users = users.Where(y => !y.Removed.Value).ToList();

            users.ForEach(x => x.Password = null);

            return users;
        }

        public async Task<User> Save(User user)
        {
            var newUser = await _repo.Save(user);

            newUser.Password = null;

            return newUser;
        }

        public async Task Update(User user)
        {
            await _repo.Update(user);
        }

        private object TokenGenerate (User user)
        {
            ClaimsIdentity identity = new ClaimsIdentity (
                new []
                {
                    new Claim ("userId", user.Id.ToString ()),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ())
                }
            );

            DateTime criationDate = DateTime.Now;

            DateTime expireDate = criationDate +
                TimeSpan.FromDays (_tokenConfigurations.Days);

            var handler = new JwtSecurityTokenHandler ();

            var key = new SymmetricSecurityKey (
                Encoding.UTF8.GetBytes (_tokenConfigurations.Key));

            var credential = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);

            var securityToken = handler.CreateToken (new SecurityTokenDescriptor {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = credential, // _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = criationDate,
                Expires = expireDate
            });

            var token = handler.WriteToken (securityToken);

            return new
            {
                created = criationDate.ToString ("yyyy-MM-dd HH:mm:ss"),
                expiration = expireDate.ToString ("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = user.Name,
                userId = user.Id
            };
        }
    }
}