using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
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
        public void SaveToken(string email, string token);
        public VolunteeringMissionView GetMission();
        public void AddToFavourite(int mid, int uid);
        public void applyvol(int userId, int missionId);

        public void UpdateRating(int ratingScore,int missionId,int userId);
        /* public UserProfileView GetUserProfile();*/
        /* public void GetUserById(int uid);
         public void GetAllUserDetails(int uid);
         public void SaveAllDetails(int uid, string fname, string lname, string empid, string title, string department, string profiletxt, string volunteertxt, int country, int city, string linkedinurl, string hiddentext);
 */

        /*   public User FindUserByEmail(string email);
           public void AddPassResetToken(PasswordReset passwordReset);
           public PasswordReset FindPassResetToken(string token);
           public void UpdateUser(User user);
   */
    }
}
