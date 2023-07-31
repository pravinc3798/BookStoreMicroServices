using BookStore.User.Entity;
using BookStore.User.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.User.Services;

public class User : IUser
{
    private readonly UserContext _db;
    private readonly IConfiguration _config;

    public User(UserContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public UserEntity Register(UserEntity newUser)
    {
        newUser.EmailId = newUser.EmailId.ToLower();
        _db.Add(newUser);
        _db.SaveChanges();
        return newUser;
    }
    public string Login(string email, string password)
    {
        email = email.ToLower();
        UserEntity user = _db.Users.FirstOrDefault(x => x.EmailId == email && x.UserPassword == password);

        if (user != null)
        {
            string token = GenerateToken(user.UserId, user.EmailId);
            return token;
        }
        else
            return null;
    }

    public UserEntity GetUserDetails(int userId)
    {
        UserEntity loggedInUser = _db.Users.FirstOrDefault(x => x.UserId == userId);

        if (loggedInUser != null)
        {
            loggedInUser.UserPassword = null;
            return loggedInUser;
        }
        else
            return null;
    }
    public bool ForgetPassword(string email)
    {
        email = email.ToLower();
        throw new NotImplementedException();
    }
    public bool ResetPassword(string password, string confirmPassword)
    {
        throw new NotImplementedException();
    }

    private string GenerateToken(int userId, string email)
    {
        byte[] key = Encoding.ASCII.GetBytes(_config["JWT-Key"]);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim (ClaimTypes.Role, "User"),
                new Claim (ClaimTypes.Email, email),
                new Claim (ClaimTypes.Sid, userId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}
