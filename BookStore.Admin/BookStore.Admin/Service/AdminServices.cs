using BookStore.Admin.Entity;
using BookStore.Admin.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Admin.Service;

public class AdminServices : IAdmin
{
    private readonly AdminContext _db;
    private readonly IConfiguration _config;

    public AdminServices(AdminContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public AdminEntity RegisterAdmin(AdminEntity newAdmin)
    {
        _db.AdminTable.Add(newAdmin);
        _db.SaveChanges();
        return newAdmin;
    }
    public string Login(string email, string password)
    {
        email = email.ToLower();
        AdminEntity admin = _db.AdminTable.FirstOrDefault(x => x.AdminEmail == email && x.AdminPassword == password);

        if (admin != null)
        {
            string token = GenerateToken(admin.AdminId, admin.AdminEmail);
            return token;
        }
        else
            return null;
    }

    private string GenerateToken(int userId, string email)
    {
        byte[] key = Encoding.ASCII.GetBytes(_config["JWT-Key"]);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim (ClaimTypes.Role, "Admin"),
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
