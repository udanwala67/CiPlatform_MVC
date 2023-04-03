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
            
            StoryView storyView = new StoryView();
            storyView.stories = Story;
            storyView.mission= Mission;
            storyView.user = User;
            storyView.missionApplication = MissionApplications;
         

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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedFiles",FileName);
                var fileStream = new FileStream(path, FileMode.Create);
                files.CopyTo(fileStream);
                string ModelPath = "/UploadedFiles/" + FileName;
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





    }
}
