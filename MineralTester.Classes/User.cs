namespace MineralTester.Classes
{
    class User
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

        public string AccountType
        {
            get;
            set;
        }

        public User(int id, string firstName, string lastName, string username, string password, string accountType)
        {
            _ID = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            AccountType = accountType;
        }
    }
}
