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



        public StoryView GetStory(long StoryId)
        {

            var Story = _CiContext.Stories.ToList();
            var User = _CiContext.Users.ToList();
            var Mission = _CiContext.Missions.ToList();
            var MissionApplications = _CiContext.MissionApplications.ToList();
            var timesheet = _CiContext.Timesheets.ToList();
            StoryView storyView = new StoryView();
            storyView.stories = Story;
            storyView.mission = Mission;
            storyView.user = User;
            storyView.missionApplication = MissionApplications;
            storyView.timesheet = timesheet;
            var thisStory = _CiContext.Stories.Include(story => story.StoryMedia).FirstOrDefault(story => story.StoryId == StoryId);
            if (thisStory != null)
            {
                storyView.StoryId = thisStory.StoryId;
                storyView.Title = thisStory.Title;
                storyView.Description = thisStory.Description;
                storyView.mid = (int)thisStory.MissionId;
                storyView.url = thisStory.StoryMedia.FirstOrDefault(media => media.Type == ".mp4")?.Path;
                storyView.Status = thisStory.Status;
            }
            return storyView;
        }


        public void SubmitStory(long storyId)
        {
            var story = _CiContext.Stories.FirstOrDefault(story => story.StoryId == storyId);
            if(story != null)
            {
                story.Status = "PUBLISHED";
                story.UpdatedAt = DateTime.Now;
                story.PublishedAt = DateTime.Now;

                _CiContext.Stories.Update(story);
                _CiContext.SaveChanges();
            }
            
        }

        public long PushStory(long userid, StoryView storyView)
        {
            var exist = _CiContext.Stories.OrderBy(story => story.CreatedAt).LastOrDefault(s => s.UserId == userid && s.MissionId == storyView.mid);
            long StoryId = 0;
            if (storyView.StoryId == 0 || exist == null)
            {
                var story = new Story()
                {
                    UserId = userid,
                    MissionId = storyView.mid,
                    Title = storyView.Title,
                    Description = storyView.Description,
                    Status = "DRAFT",
                    // PublishedAt = storyView.date,
                    CreatedAt = DateTime.Now
                };
                _CiContext.Stories.Add(story);
                _CiContext.SaveChanges();
                StoryId = story.StoryId;

            }
            else
            {
                exist.Title = storyView.Title;

                exist.Description = storyView.Description;
                exist.UpdatedAt = DateTime.Now;
                exist.MissionId = storyView.mid;


                _CiContext.Stories.Update(exist);
                _CiContext.SaveChanges();

                StoryId = exist.StoryId;
            }


            foreach (var file in storyView.UploadedFiles)
            {
                //Getting FileName
                var fileName = Path.GetFileName(file.FileName);

                //Assigning Unique Filename (Guid)
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                // concatenating  FileName + FileExtension
                var newFileName = String.Concat(myUniqueFileName, fileExtension);

                // Combines two strings into a path.

                var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", "Story", StoryId.ToString());

                DirectoryInfo folderInfo = Directory.CreateDirectory(folderpath);

                var filepath = folderInfo + $@"\{newFileName}";

                using FileStream fs = System.IO.File.Create(filepath);
                file.CopyTo(fs);
                fs.Flush();
                //string FileName = Path.GetFullPath(files.FileName);
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//UploadedFiles",FileName);
                //var fileStream = new FileStream(path, FileMode.Create);
                //files.CopyTo(fileStream);
                //string ModelPath = "/UploadedFiles/" + FileName;
                StoryMedium storyMedium = new StoryMedium()
                {
                    StoryId = StoryId,
                    Path = newFileName,
                    Type = fileExtension,
                    CreatedAt = DateTime.Now

                };
                _CiContext.StoryMedia.Add(storyMedium);
            }
            var storyUrl = _CiContext.StoryMedia.FirstOrDefault(media => media.StoryId == StoryId && media.Type == ".mp4");
            if (storyUrl != null)
            {
                _CiContext.StoryMedia.Remove(storyUrl);
            }
            if (storyView.url != null)
            {
                var storyMedia = new StoryMedium()
                {
                    StoryId = StoryId,
                    Path = storyView.url,
                    Type = ".mp4",
                    CreatedAt = DateTime.Now
                };
                _CiContext.StoryMedia.Add(storyMedia);
            }
            _CiContext.SaveChanges();

            return StoryId;
        }

        public VolunteeringTimesheetView GetAllMissions(int uid)
        {
            var missionApp = _CiContext.MissionApplications.Where(a => a.DeletedAt == null).Include(m => m.Mission).ToList();
            var timesheet = _CiContext.Timesheets.Where(t => t.UserId == uid).Include(t => t.Mission).ToList();
            var user = _CiContext.Users.ToList();
            VolunteeringTimesheetView model = new VolunteeringTimesheetView();
            model.missionApplications = missionApp;
            model.timesheets = timesheet;
            model.user = user;
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
                timesheet.CreatedAt = DateTime.Now;
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
                timesheet.CreatedAt= DateTime.Now;  
                _CiContext.Timesheets.Add(timesheet);
            }
            else
            {
                var timesheet = _CiContext.Timesheets.Where(t => t.TimesheetId == model.tid).FirstOrDefault();
                timesheet.MissionId = model.MissionId;
                timesheet.Action = model.action;
                timesheet.DateVolunteered = model.date;
                timesheet.Notes = model.message;
                _CiContext.Timesheets.Update(timesheet);
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

