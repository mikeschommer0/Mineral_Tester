/// <summary>
/// Written by Seth Frevert
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Interface for the user manager
    /// </summary>
    public interface IUserManager
    {
        int AddUser(User newUser);

        int UpdateUser(User userToUpdate);

        int DeleteUser(User userToDelete);
    }
}