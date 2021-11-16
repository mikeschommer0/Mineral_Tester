namespace MineralTester.Classes
{
    public interface IUserManager
    {
        int AddUser(User newUser);

        int UpdateUser(User userToUpdate);

        int DeleteUser(User userToDelete);
    }
}