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
        public string FirstName { get ; set ; }
        public string LastName { get ; set ; }
        public string Password { get ; set ; }
        public string Login { get ; set ; } 
        public UserLevel LevelUserLevel { get ; set ; }

    }
}
