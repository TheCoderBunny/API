using API.Models;

namespace API.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserByEmail(string email);
    User CreateUser(User newUser);
    string SignInUser(string email, string password);
    User? UpdateUser(User newUser);
    void DeleteUserById(int userId);
}