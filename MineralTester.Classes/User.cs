namespace MineralTester.Classes
{
    public class User
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string FullName
        {
            get => FirstName + " " + LastName;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public Enums.AccountType AccountType
        {
            get;
            set;
        }

        public string Salt
        {
            get;
            set;
        }

        public User(int id, string firstName, string lastName, string username, string password, Enums.AccountType accountType)
        {
            _ID = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            AccountType = accountType;
            Salt = SecurityHelper.GenerateSalt();
            Password = SecurityHelper.HashPassword(password, Salt);
        }

        public User(int id, string firstName, string lastName, string username, string password, Enums.AccountType accountType, string salt)
        {
            _ID = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            AccountType = accountType;
            Salt = salt;
            Password = password;
        }
    }
}
