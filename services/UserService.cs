using _3ASPC_API.database;
using _3ASPC_API.models;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC_API.services;

public class UserService
{
    private readonly ApiContext _context;

    public UserService(ApiContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new Exception("Email already exists.");
        }

        if (await _context.Users.AnyAsync(u => u.Pseudo == user.Pseudo))
        {
            throw new Exception("Pseudo already exists.");
        }

        var newUser = new User
        {
            Email = user.Email,
            Pseudo = user.Pseudo,
            Password = user.Password,
            Role = user.Role
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (user.Password != password)
        {
            throw new Exception("Invalid password.");
        }

        return user;
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}