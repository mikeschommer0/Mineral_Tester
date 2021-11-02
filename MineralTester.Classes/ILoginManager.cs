namespace MineralTester.Classes
{
    public interface ILoginManager
    {
        User Login(string username, string password);
        User Logout(ref User user);
    }
}