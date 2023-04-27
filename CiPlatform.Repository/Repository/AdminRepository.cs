using CiPlatform.Entitites.Data;
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

        public AdminView GetAdmin()
        {
            
            var User = _CiContext.Users.ToList();
            var AdminView = new AdminView();
            AdminView.user = User;  
            return AdminView;

        }
    }
}
