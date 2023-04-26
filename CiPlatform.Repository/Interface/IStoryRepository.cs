using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository.Interface
{
    public interface IStoryRepository
    {

        public StoryView GetStory();
        public void PushStory(long userid, StoryView storyView);
        public VolunteeringTimesheetView GetAllMissions(int uid);
        public void AddTimeTimesheet(VolunteeringTimesheetView model, int uid);
        public void AddGoalTimesheet(VolunteeringTimesheetView model, int uid);
        public void deleteTimesheet(int tid);
        public Timesheet GetTimesheetById(int tid);
        public VolunteeringTimesheetView getRange(int missionId);
        public void changeStatusById(int storyId);
     




    }
}

