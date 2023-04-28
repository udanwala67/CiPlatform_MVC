using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Interface
{
    public interface IUserDetailsRepository
    {
        public UserProfileView GetUserProfile(int id);

        public User GetUserById(int id);
        
        public void SaveAllDetails(int uid, string fname, string lname, string employeeid, string title, string department, string profiletext, string volunteertext, int country, int city, string linkedinurl, string hiddentext);

        public void updatePass(int uid, string oldpass, string newpass, string cnewpass);
        public User GetUserPhotoById(int uid);


    }


}
