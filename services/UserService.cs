using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _3ASPC_API.database;
using _3ASPC_API.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace _3ASPC_API.services;

public class UserService
{
    private readonly ApiContext _context;
    public IConfiguration Configuration { get; }

    public UserService(ApiContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }

    public async Task<User> RegisterAsync(UserRequest user)
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
            Password = BC.HashPassword(user.Password),
            Role = user.Role
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (!BC.Verify(password, user.Password))
        {
            throw new Exception("Invalid password.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserId", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = Configuration["Jwt:Issuer"],
            Audience = Configuration["Jwt:Audience"],
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? string.Empty)),
                    SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> UpdateUserAsync(int id, UserRequest user)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
        {
            throw new Exception("User not found.");
        }

        if (user.Email != null && user.Email != existingUser.Email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("Email already exists.");
            }
        }

        if (user.Pseudo != null && user.Pseudo != existingUser.Pseudo)
        {
            if (await _context.Users.AnyAsync(u => u.Pseudo == user.Pseudo))
            {
                throw new Exception("Pseudo already exists.");
            }
        }

        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.Pseudo = user.Pseudo ?? existingUser.Pseudo;
        existingUser.Password = BC.HashPassword(user.Password) ?? existingUser.Password;
        existingUser.Role = user.Role ?? existingUser.Role;
        await _context.SaveChangesAsync();

        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}