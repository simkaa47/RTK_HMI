using System.Collections.Generic;

namespace DataAccess.Models
{
    public static class UserDataFactory
    {
        public static IEnumerable<User> Get()
        {
            return new List<User>()
            {
                new User{FirstName="Иван",LastName="Власов", Password="Холин", Level= UserLevel.Service, Login="service"},
                new User{FirstName="Админ",LastName="Админов", Password="0000", Level= UserLevel.Admin, Login="admin"}
            };
        }
    }
}
