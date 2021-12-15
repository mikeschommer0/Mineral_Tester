/// <summary>
/// Written by Seth Frevert
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Interface for the login manager
    /// </summary>
    public interface ILoginManager
    {
        User Login(string username, string password);
        User Logout(ref User user);
    }
}