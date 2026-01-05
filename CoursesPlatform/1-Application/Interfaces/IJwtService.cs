using _2_Domain.Entities;

namespace _1_Application.Interfaces;

public interface IJwtService
{ 
    string GenerateToken(User user);
}