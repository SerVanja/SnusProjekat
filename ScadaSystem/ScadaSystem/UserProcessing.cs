using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public static class UserProcessing 
    {
        static List<User> UserList;


        public static void LoadUsers()
        {
            using (var db = new UserDatabase())
            {
                if (db.Users.Count() != 0)
                {
                    UserList = db.Users.ToList();
                }
                else {
                    UserList = new List<User>();
                }

            }
        }


        public static List<User> GetUsers() {
            Console.WriteLine(UserList);
            return UserList;
        }


        public static void AddUser(User newUser)
        {
            using (var db = new UserDatabase())
            {
                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }
    }
}
