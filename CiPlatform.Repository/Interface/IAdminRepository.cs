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
    public interface IAdminRepository
    {
        public AdminView GetData();
        public void AddUser(AdminView model);
        public User GetUser(int userId);
        public void deleteUser(int uid);
      /*  public GetApplicationRecords(int pageNumber, int pageSize, string keyword);*/
    }
}
