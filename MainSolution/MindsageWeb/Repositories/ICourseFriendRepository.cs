using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    public interface ICourseFriendRepository
    {
        #region Methods

        CourseFriend GetCourseFriendByUserProfile(string userProfileName);

        #endregion Methods
    }
}
