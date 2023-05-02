using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CiContext _CiContext;
        public AdminRepository(CiContext CiContext)
        {
            _CiContext = CiContext;
        }

        public AdminView GetData()
        {

            var users = _CiContext.Users.Where(user => user.DeletedAt == null).ToList();
            var cmspages = _CiContext.CmsPages.ToList();
            var missions = _CiContext.Missions.ToList();
            var missionApplications = _CiContext.MissionApplications.ToList();
            var missionThemes = _CiContext.MissionThemes.ToList();
            var missionskill = _CiContext.MissionSkills.ToList();
            var country = _CiContext.Countries.ToList();
            var city = _CiContext.Cities.ToList();
            var skills = _CiContext.Skills.ToList();
            var stories = _CiContext.Stories.ToList();
            var banners = _CiContext.Banners.ToList();

            var model = new AdminView();
            model.users = users;
            model.cmspages = cmspages;
            model.missions = missions;
            model.missionApplications = missionApplications;
            model.missionThemes = missionThemes;
            model.missionSkills = missionskill;
            model.country = country;
            model.city = city;
            model.skills = skills;
            model.stories = stories;
            model.banners = banners;
            return model;

        }

        public void AddUser(AdminView model)
        {
            if (model.UserId == 0)
            {
                var user = new User();
                user.FirstName = model.user.FirstName;
                user.LastName = model.user.LastName;
                user.Email = model.user.Email;
                user.Password = model.user.Password;
                user.Department = model.user.Department;
                user.ProfileText = model.user.ProfileText;
                user.CityId = model.user.CityId;
                user.CountryId = model.user.CountryId;
                user.Status = model.user.Status;
                user.CreatedAt = DateTime.Now;
                if (model.AvatarImage != null)
                {
                    string filename = Path.GetFileName(model.AvatarImage.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Avatars", filename);
                    var filestream = new FileStream(uploadPath, FileMode.Create);
                    model.AvatarImage.CopyTo(filestream);
                    string dbfilepath = "/Avatars/" + filename;
                    user.Avatar = dbfilepath;
                }
                _CiContext.Users.Add(user);
            }
            else
            {
                var user = _CiContext.Users.Where(user => user.UserId == model.UserId).FirstOrDefault();
                user.FirstName = model.user.FirstName;
                user.LastName = model.user.LastName;
                user.Email = model.user.Email;
                user.Password = model.user.Password;
                user.Department = model.user.Department;
                user.ProfileText = model.user.ProfileText;
                user.CityId = model.user.CityId;
                user.CountryId = model.user.CountryId;
                user.Status = model.user.Status;
                user.CreatedAt = DateTime.Now;
                if (model.AvatarImage != null)
                {
                    string filename = Path.GetFileName(model.AvatarImage.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Avatars", filename);
                    var filestream = new FileStream(uploadPath, FileMode.Create);
                    model.AvatarImage.CopyTo(filestream);
                    string dbfilepath = "/Avatars/" + filename;
                    user.Avatar = dbfilepath;
                }
                _CiContext.Users.Update(user);
            }
            _CiContext.SaveChanges();
        }


        public User GetUser(int userId)
        {
            return _CiContext.Users.FirstOrDefault(x => x.UserId == userId);
        }

        public void deleteUser(int uid)
        {
            var user = _CiContext.Users.FirstOrDefault(x => x.UserId == uid);
            if (user != null)
            {
                user.DeletedAt = DateTime.Now;
                _CiContext.Users.Update(user);
            }
            _CiContext.SaveChanges();
        }
       
    }
}
