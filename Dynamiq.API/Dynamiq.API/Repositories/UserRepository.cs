﻿using AutoMapper;
using Dynamiq.API.DAL.Context;
using Dynamiq.API.DAL.Models;
using Dynamiq.API.Extension.DTOs;
using Dynamiq.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dynamiq.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UserRepository(IMapper mapper, AppDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task Delete(Guid id)
        {
            var model = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                throw new ArgumentException($"{nameof(id)} does not exist");

            _db.Users.Remove(model);

            await _db.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAll()
        {
            var models = await _db.Users.ToListAsync();

            return _mapper.Map<List<UserDto>>(models);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var model = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (model == null)
                throw new ArgumentException($"user with the email: {nameof(email)} does not exist");

            return _mapper.Map<UserDto>(model);
        }

        public async Task<UserDto> GetById(Guid id)
        {
            var model = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                throw new ArgumentException($"user with the id: {nameof(id)} does not exist");

            return _mapper.Map<UserDto>(model);
        }

        public async Task<UserDto> Insert(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentNullException(nameof(user));

            var model = new User()
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Role = user.Role,
                ConfirmedEmail = false,
                PasswordHash = user.PasswordHash
            };

            _db.Users.Add(model);

            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(model);
        }

        public async Task<UserDto> Update(UserDto user)
        {
            var newDataUser = await _db.Users.FindAsync(user.Id);
            if (newDataUser == null)
                throw new ArgumentException("User was not found");

            newDataUser.Email = user.Email;
            newDataUser.Role = user.Role;
            newDataUser.ConfirmedEmail = user.ConfirmedEmail;
            newDataUser.PasswordHash = user.PasswordHash;
            newDataUser.Role = user.Role;

            _db.Users.Update(newDataUser);

            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(newDataUser);
        }

    }
}
