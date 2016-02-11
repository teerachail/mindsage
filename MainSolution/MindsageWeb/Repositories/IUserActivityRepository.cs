using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    public interface IUserActivityRepository
    {
        #region Methods

        UserActivity GetUserActivityByUserProfile(string userprofile);
        void UpsertUserActivity(UserActivity data);

        #endregion Methods
    }
}
