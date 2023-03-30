﻿using CiPlatform.Entitites.Data;
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
            
            StoryView storyView = new StoryView();
            storyView.stories = Story;
            storyView.mission= Mission;
            storyView.user = User;
         

            return storyView;
        }

        public void PushStory(long userid ,int mid,string storytitle)
        {
            var story = new Story()
            {
                UserId = userid,
                MissionId = mid,
                Title = storytitle,
                Status = "PUBLISHED",
                CreatedAt = DateTime.Now
            };
            _CiContext.Stories.Add(story);
            _CiContext.SaveChanges();
            
            
        }
   
        
       
    }
}
