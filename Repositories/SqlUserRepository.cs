using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Migrations;
using API.Models;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using bcrypt = BCrypt.Net.BCrypt;

namespace API.Repositories;

public class SqlUserRepository : IUserRepository
{
    private static IConfiguration _config;
    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";

    public SqlUserRepository(IConfiguration config)
    {
        _config = config;
    }

    private string BuildToken(User user)
    {
        var secret = _config.GetValue<String>("TokenSecret");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // Create Signature using secret signing key
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Create claims to add to JWT payload
        var claims = new Claim[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.email ?? ""),
        new Claim(JwtRegisteredClaimNames.FamilyName, user.lastName ?? ""),
        new Claim(JwtRegisteredClaimNames.GivenName, user.firstName ?? "")
        };

        // Create token
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: signingCredentials);

        // Encode token
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    public User CreateUser(User newUser)
    {
        var passwordHash = bcrypt.HashPassword(newUser.password);
        newUser.password = passwordHash;

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "INSERT INTO user (firstName, lastName, email, password, userType) VALUES (@firstName, @lastName, @email, @password, @userType);";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@firstName", newUser.firstName);
        command.Parameters.AddWithValue("@lastName", newUser.lastName);
        command.Parameters.AddWithValue("@email", newUser.email);
        command.Parameters.AddWithValue("@password", newUser.password);
        command.Parameters.AddWithValue("@userType", 0);//by default this is 0 until someone with admin access changes their user priviledge status
        command.Prepare();

        command.ExecuteNonQuery();
        int id = (int)command.LastInsertedId;
        conn.Close();

        if (id > 0)
        {
            newUser.userId = id;
            return newUser;
        }

        return newUser;
    }

    public string SignInUser(string email, string password)
    {
        var user = GetUserByEmail(email);

        var verified = false;

        if (user != null)
        {
            verified = bcrypt.Verify(password, user.password);
        }

        if (user == null || !verified)
        {
            return String.Empty;
        }

        // Create & return JWT
        return BuildToken(user);
    }

    public void DeleteUserById(int userId)
    {
        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "DELETE FROM user WHERE userId = @id;";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@id", userId);
        command.Prepare();

        command.ExecuteNonQuery();

        conn.Close();
    }

    public IEnumerable<User> GetAllUsers()
    {
        var userList = new List<User>();

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "SELECT * FROM user;";
        var command = new MySqlCommand(query, conn);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            userList.Add(new User
            {
                userId = reader.GetInt32("userId"),
                firstName = reader.GetString("firstName"),
                lastName = reader.GetString("lastName"),
                email = reader.GetString("email"),
                password = reader.GetString("password"),
                userType = reader.GetInt32("userType")
            });
        }

        reader.Close();
        conn.Close();

        return userList;
    }

    public User? GetUserByEmail(string email)
    {
        User user = null;

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "SELECT * FROM user WHERE email = @email;";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@email", email);
        command.Prepare();

        var reader = command.ExecuteReader();

        reader.Read();

        if (reader.HasRows)
        {
            user = new User
            {
                userId = reader.GetInt32("userId"),
                firstName = reader.GetString("firstName"),
                lastName = reader.GetString("lastName"),
                email = reader.GetString("email"),
                password = reader.GetString("password"),
                userType = reader.GetInt32("userType")
            };
        }

        reader.Close();
        conn.Close();

        return user;
    }

    public User? UpdateUser(User newUser)
    {
        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "UPDATE user SET firstName = @firstName, lastName = @lastName, email = @email, password = @password, userType = @userType " +
            "WHERE userId = @id";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@firstName", newUser.firstName);
        command.Parameters.AddWithValue("@lastName", newUser.lastName);
        command.Parameters.AddWithValue("@email", newUser.email);
        command.Parameters.AddWithValue("@password", newUser.password);
        command.Parameters.AddWithValue("@userType", newUser.userType);
        command.Prepare();

        command.ExecuteNonQuery();

        conn.Close();

        return newUser;
    }
}
