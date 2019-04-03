using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using Repository.Data.Migrations;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class UserRepository:BaseRepository<User>
    {
        public User GetByUserPass(string username, string password)
        {
            if (username == "" || password == "")
                throw new LocalException("Username or password is empty", "نام کاربری یا کلمه عبور خالی است");

            var result = from u in base.MTOContext.Users
                where u.Username == username && u.Password == password
                select u;

            return result.FirstOrDefault();
        }
    }
}
