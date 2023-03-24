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
using CiPlatform.Entitites.ViewModels;

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

        public VolunteeringMissionView GetMission()
        {
            var missions = _CiContext.Missions.ToList();
            var cities = _CiContext.Cities.ToList();
            var country = _CiContext.Countries.ToList();
            var goalMissions = _CiContext.GoalMissions.ToList();
            var favouriteMission = _CiContext.FavoriteMissions.ToList();
            var missionRating = _CiContext.MissionRatings.ToList();
            var missiontheme = _CiContext.MissionThemes.ToList();
            var user = _CiContext.Users.ToList();
            var comments = _CiContext.Comments.ToList();
            var missionapplication = _CiContext.MissionApplications.ToList();
            var missiondocument = _CiContext.MissionDocuments.ToList();
            VolunteeringMissionView mission = new VolunteeringMissionView();
            mission.mission = missions;

            mission.city = cities;
            mission.country = country;
            mission.goalMission = goalMissions;
            mission.favoriteMission = favouriteMission;
            mission.missionRating = missionRating;
            mission.missionTheme = missiontheme;
            mission.user = user;
            mission.comments = comments;
            mission.application = missionapplication;
            mission.document = missiondocument;
            return mission;
        }

        public void AddToFavourite(int uid, int mid)
        {
            var sahil = _CiContext.FavoriteMissions.Where(f => f.UserId == uid && f.MissionId == mid).FirstOrDefault();
            if (sahil == null)
            {
                FavoriteMission favourite = new FavoriteMission();
                favourite.MissionId = mid;
                favourite.UserId = uid;
                favourite.CreatedAt = DateTime.Now;

                _CiContext.Add(favourite);
                _CiContext.SaveChanges();
            }
            else
            {
                _CiContext.Remove(sahil);
                _CiContext.SaveChanges();
            }

        }

        public void applyvol(int userId, int missionId)
        {
            MissionApplication missionApplication = new MissionApplication();
            missionApplication.UserId = userId;
            missionApplication.MissionId = missionId;
            _CiContext.Add(missionApplication);
            _CiContext.SaveChanges();
        }

    }
}

