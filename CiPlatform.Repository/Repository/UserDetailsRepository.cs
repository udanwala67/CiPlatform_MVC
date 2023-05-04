using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var userskill = _CiContext.UserSkills.Where(skill => skill.UserId == uid).ToList();
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
            _CiContext.SaveChanges();
            if(hiddentext != null)
            {
                _CiContext.Database.ExecuteSqlRaw($"delete from user_skill where user_id={uid}");
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

        public void UpdateUserPhoto(int userId, string fileName)
        {
            var user = _CiContext.Users.Where(u => u.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                // Delete the old profile photo file, if it exists
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", user.Avatar);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update the user's profile photo file name in the database
                user.Avatar = fileName;
                _CiContext.SaveChanges();
            }
        }

        public User GetUserPhotoById(int uid)
        {
            var userp = _CiContext.Users.FirstOrDefault(u => u.UserId == uid);
            return userp;
          /*  if (userp != null)
            {
                // Get the file name from the user's Avatar property
                string fileName = Path.GetFileName(userp);

                // Construct the full path to the file on the server
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Return the file as a FileStreamResult
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                var fileStreamResult = new FileStreamResult(fileStream, "image/jpeg");
                return;
            }
            else
            {
                return;
            }*/
        }





    }
}
