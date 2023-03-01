using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Repository
{
    public class CiRepository : ICiRepository
    {
        public readonly CiContext _ciContext;

        public CiRepository(CiContext ciContext)
        {
            _ciContext = ciContext;
        }


        public void RegisterUser(User user)
        {
            var data = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Password = user.Password,
                CityId = 1,
                CountryId = 1,
            };

            _ciContext.Users.Add(data);
            _ciContext.SaveChanges();
        }

        public User GetUserEmail(string email)
        {
            return _ciContext.Users.FirstOrDefault(u => u.Email == email);
        }


        /*   public List<User> GetUserData()
         {
             List<User> users = _ciContext.Users.ToList();
             return users;
         }
       */

    }
}