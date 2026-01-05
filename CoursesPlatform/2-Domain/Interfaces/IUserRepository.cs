using _2_Domain.Entities;

namespace _2_Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

}