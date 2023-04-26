using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly CiContext _CiContext;
        public StoryRepository(CiContext CiContext)
        {
            _CiContext = CiContext;
        }



        public StoryView GetStory()
        {

            var Story = _CiContext.Stories.ToList();
            var User = _CiContext.Users.ToList();
            var Mission = _CiContext.Missions.ToList();
            var MissionApplications = _CiContext.MissionApplications.ToList();
            var timesheet = _CiContext.Timesheets.ToList();
            StoryView storyView = new StoryView();
            storyView.stories = Story;
            storyView.mission= Mission;
            storyView.user = User;
            storyView.missionApplication = MissionApplications;
            storyView.timesheet = timesheet;

            return storyView;
        }

        public void PushStory(long userid ,StoryView storyView)
        {
            var exist = _CiContext.Stories.FirstOrDefault(s => s.UserId == userid && s.MissionId == storyView.mid);
            if(exist == null)
            {
                var story = new Story()
                {
                    UserId = userid,
                    MissionId = storyView.mid,
                    Title = storyView.Title,
                    Status = "PUBLISHED",
                    // PublishedAt = storyView.date,
                    CreatedAt = DateTime.Now
                };
                _CiContext.Stories.Add(story);

            }
            else
            {
                exist.Title = storyView.Title;

                exist.Description = storyView.Description;
                //exist.UpdatedAt = DateTime.Now;

            }

           
            _CiContext.SaveChanges();

            long StoryId = _CiContext.Stories.Where(s => s.UserId == userid && s.MissionId == storyView.mid).Select(x => x.StoryId).FirstOrDefault();
            foreach(var files in storyView.UploadedFiles)
            {
                string FileName = Path.GetFullPath(files.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//UploadedFiles",FileName);
                var fileStream = new FileStream(path, FileMode.Create);
                files.CopyTo(fileStream);
                string ModelPath = "/UploadedFiles" + FileName;
                StoryMedium storyMedium = new StoryMedium()
                {
                    StoryId = StoryId,
                    Path = ModelPath,
                    Type = "png",
                    CreatedAt = DateTime.Now

                };
                _CiContext.StoryMedia.Add(storyMedium);
                _CiContext.SaveChanges();
            }
          
        }

        public VolunteeringTimesheetView GetAllMissions(int uid)
        {
            var missionApp = _CiContext.MissionApplications.Where(a => a.DeletedAt == null).Include(m => m.Mission).ToList();
            var timesheet = _CiContext.Timesheets.Where(t => t.UserId == uid).Include(t => t.Mission).ToList();
            VolunteeringTimesheetView model = new VolunteeringTimesheetView();
            model.missionApplications = missionApp;
            model.timesheets = timesheet;
            return model;
        }

        public void AddTimeTimesheet(VolunteeringTimesheetView model, int uid)
        {
            if (model.tid == 0)
            {
                var timesheet = new Timesheet();
                timesheet.UserId = uid;
                timesheet.MissionId = model.MissionId;
                timesheet.Time = TimeSpan.Parse(model.hours.ToString() + ":" + model.minutes.ToString() + ":00");
                timesheet.DateVolunteered = model.date;
                timesheet.Notes = model.message;
                timesheet.CreatedAt=DateTime.Now;
                _CiContext.Timesheets.Add(timesheet);
            }
            else
            {
                var timesheet = _CiContext.Timesheets.Where(t => t.TimesheetId == model.tid).FirstOrDefault();
                timesheet.MissionId = model.MissionId;
                timesheet.Time = TimeSpan.Parse(model.hours.ToString() + ":" + model.minutes.ToString() + ":00");
                timesheet.DateVolunteered = model.date;
                timesheet.Notes = model.message;
                timesheet.CreatedAt = DateTime.Now;
                _CiContext.Timesheets.Update(timesheet);
                
            }
            _CiContext.SaveChanges();
        }

        public void AddGoalTimesheet(VolunteeringTimesheetView model, int uid)
        {
            if (model.tid == 0)
            {
                var timesheet = new Timesheet();
                timesheet.UserId = uid;
                timesheet.MissionId = model.MissionId;
                timesheet.Action = model.action;
                timesheet.DateVolunteered = model.date;
                timesheet.Notes = model.message;
                _CiContext.Timesheets.Add(timesheet);
            }
            else
            {
                var timesheet = _CiContext.Timesheets.Where(t => t.TimesheetId == model.tid).FirstOrDefault();
                timesheet.MissionId = model.MissionId;
                timesheet.Action = model.action;
                timesheet.DateVolunteered = model.date;
                timesheet.Notes = model.message;
            }
            _CiContext.SaveChanges();
        }
        
        public void deleteTimesheet(int tid)
        {
            var timesheet = _CiContext.Timesheets.Where(t => t.TimesheetId == tid).FirstOrDefault();
            _CiContext.Timesheets.Remove(timesheet);
            _CiContext.SaveChanges();
        }
        public Timesheet GetTimesheetById(int tid)
        {
            var timesheet = _CiContext.Timesheets.Where(t => t.TimesheetId == tid).Include(t => t.Mission).FirstOrDefault();
            return timesheet;
        }

        public VolunteeringTimesheetView getRange(int missionId)
        {
            var mission = _CiContext.Missions.Where(mission => mission.MissionId == missionId).FirstOrDefault();

            var model = new VolunteeringTimesheetView();
            model.startDate = (DateTime)mission.StartDate;
            model.endDate = (DateTime)mission.EndDate;

            return model;

        }

        public void changeStatusById(int storyId)
        {
            var story = _CiContext.Stories.FirstOrDefault(s => s.StoryId == storyId);
            story.Status = "PENDING";
            _CiContext.Stories.Update(story);
            _CiContext.SaveChanges();
        }
    }
}
    
