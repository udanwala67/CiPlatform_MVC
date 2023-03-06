using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace CiPlatform.Repository.Repository
{
    public class CiRepository : ICiRepository
    {
        private readonly CiContext _CiContext;

        public CiRepository(CiContext CiContext)
        {
            _CiContext = CiContext;
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
                CreatedAt = DateTime.Now,
            };

            _CiContext.Users.Add(data);
            _CiContext.SaveChanges();
        }


        public User GetUserEmail(string email)
        {
            return _CiContext.Users.FirstOrDefault(u => u.Email == email);
        }
        public void SaveToken(string email, string token)
        {
            var data = new PasswordReset()
            {
                Email = email,
                Token = token,
            };
            _CiContext.PasswordResets.Add(data);
            _CiContext.PasswordResets.Add(data);
        }
    }
}

