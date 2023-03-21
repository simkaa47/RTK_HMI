namespace DataAccess.Models
{
    public enum UserLevel
    {
        None,
        Service,
        Admin
    }

    public class User : PropertyChangedBase, IDatabased
    {
        public int Id { get ; set ; }

        #region First name
        /// <summary>
        /// First name
        /// </summary>
        private string _firstName;
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }
        #endregion

        #region Last name
        /// <summary>
        /// Last name
        /// </summary>
        private string _lastName;
        /// <summary>
        /// Last name
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }
        #endregion

        #region Password
        /// <summary>
        /// Password
        /// </summary>
        private string _password;
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
        #endregion

        #region Login
        /// <summary>
        /// Login
        /// </summary>
        private string _login;
        /// <summary>
        /// Login
        /// </summary>
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        #endregion

        #region Level
        /// <summary>
        /// Level
        /// </summary>
        private UserLevel _level;
        /// <summary>
        /// Level
        /// </summary>
        public UserLevel Level
        {
            get => _level;
            set => Set(ref _level, value);
        }
        #endregion        

    }
}
