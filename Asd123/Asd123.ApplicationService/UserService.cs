﻿using Asd123.Domain;
using Asd123.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Asd123.ApplicationService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EnsureUser(IReadOnlyCollection<Claim> claims)
        {
            var userIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdentifier == null)
            {
                throw new ArgumentException("NameIdentifier was not found.");
            }

            var user = await _userRepository.FindByIdentifier(userIdentifier.Value);

            if (user == null)
            {
                var u = new User()
                {
                    UserIdentifier = userIdentifier.Value,
                    Email = claims.First(c => c.Type == ClaimTypes.Email).Value,
                    Name = claims.First(c => c.Type == ClaimTypes.Name).Value
                };

                await _userRepository.Create(u);
            }
        }

        public async Task<User> GetById(string id)
        {
            return await _userRepository.FindByIdentifier(id);
        }
    }
}
