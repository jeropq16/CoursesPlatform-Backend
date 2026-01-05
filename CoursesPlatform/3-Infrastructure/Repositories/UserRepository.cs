using _2_Domain.Entities;
using _2_Domain.Interfaces;
using _3_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }
}