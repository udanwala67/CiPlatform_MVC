using CiPlatform.Entitites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class VolunteeringTimesheetView
    {

        public List<Timesheet> timesheets = new List<Timesheet>();
        public List<MissionApplication> missionApplications = new List<MissionApplication>();
       
       public int MissionId { get; set; }

        public List<User> user = new List<User>();  
        public int hours { get; set; }

        public int minutes { get; set; }
        public  int action { get; set; }
        public DateTime date { get; set; }
        public string message { get; set; } = string.Empty; 
        public int tid { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }

}
