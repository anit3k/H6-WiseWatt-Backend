﻿using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> CreateNewUser(UserEntity user);
        Task<UserEntity> GetUser(UserEntity user);
        Task<bool> ValidateUsernameEmail(UserEntity user);
    }
}
