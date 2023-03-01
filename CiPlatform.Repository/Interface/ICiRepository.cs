using CiPlatform.Entitites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Interface
{
    public interface ICiRepository
    {
        public void RegisterUser(User user);
        public User GetUserEmail(string email);

     /*   public User FindUserByEmail(string email);
        public void AddPassResetToken(PasswordReset passwordReset);
        public PasswordReset FindPassResetToken(string token);
        public void UpdateUser(User user);*/

    }
}
