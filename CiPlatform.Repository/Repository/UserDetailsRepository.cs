using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Model = Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CiPlatform.Repository.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly CiContext _CiContext;
        public UserDetailsRepository(CiContext CiContext)
        {
            _CiContext = CiContext;
        }

        public User GetUserById(int uid)
        {
            return _CiContext.Users.FirstOrDefault(u => u.UserId == uid);
        }
        public UserProfileView GetUserProfile(int uid)
        {
            string userAvtar = _CiContext.Users.Where(u=>u.UserId == uid).Select(u=>u.Avatar).FirstOrDefault();
            var user = _CiContext.Users.ToList();
            var skill = _CiContext.Skills.ToList();
            var userskill = _CiContext.UserSkills.ToList();
            var country = _CiContext.Countries.ToList();
            var city = _CiContext.Cities.ToList();
            
            UserProfileView profile = new UserProfileView();

            profile.user = user;
            profile.skills = skill;
            profile.country = country;
            profile.city = city;
            profile.Avatar = userAvtar;
            profile.userSkills = userskill;
            return profile;
        }

        public void SaveAllDetails(int uid,string fname, string lname, string employeeid, string title, string department, string profiletext, string volunteertext, int country, int city, string linkedinurl, string hiddentext)
        {
            var user = _CiContext.Users.Where(u => u.UserId == uid).FirstOrDefault();
            user.UserId = uid;
            user.FirstName = fname;
            user.LastName = lname;
            user.EmployeeId = employeeid;
            user.Title = title;
            user.Department = department;
            user.ProfileText = profiletext;
            user.WhyIVolunteer = volunteertext;
            user.CountryId = country;
            user.CityId = city;
            user.LinkedInUrl = linkedinurl;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _CiContext.Users.Update(user);
            _CiContext.Database.ExecuteSqlRaw($"delete from user_skill where user_id={uid}");
            _CiContext.SaveChanges();
            if(hiddentext != null)
            {
                List<int> list = new List<int>();
                string a = hiddentext.Remove(hiddentext.Length - 1,1);
                list = a.Split(',').Select(int.Parse).ToList();

                foreach (var item in list)
                {
                    var userskill = new UserSkill();
                    userskill.UserId = uid;
                    userskill.SkillId = item;
                    userskill.CreatedAt = DateTime.Now;
                    userskill.UpdatedAt = DateTime.Now;
                    _CiContext.UserSkills.Add(userskill);

                }
                _CiContext.SaveChanges();

            }
        }

        public void updatePass(int uid ,string oldpass,string newpass,string cnewpass)
        {
            var user = _CiContext.Users.Where(u => u.UserId == uid).FirstOrDefault();
            

            if (oldpass == user.Password)
            {
                if (newpass == cnewpass)
                {
                    user.Password = newpass;
                    _CiContext.Users.Update(user);
                    _CiContext.SaveChanges();
                }
            }
        }

        public void GetUserPhotoById(int uid)
        {
           /* var userp = _CiContext.Users.Where(x => x.UserId == uid).FirstOrDefault();
           *//* string filename = Path.GetFileName();*//*
            string UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedFiles", filename);
            var filestream = new FileStream(UploadPath, FileMode.Create);
            *//*Model.ImgAvatar.CopyTo(filestream);*//*
            string dbfilepath = "/UploadedFiles/";
            userp.Avatar = dbfilepath;
            _CiContext.Users.Update(userp);
            _CiContext.SaveChanges();*/

        }



    }
}
