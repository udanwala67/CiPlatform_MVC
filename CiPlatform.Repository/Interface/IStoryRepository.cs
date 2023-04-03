using CiPlatform.Entitites.ViewModels;
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


    }
}
