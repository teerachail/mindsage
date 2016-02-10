using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ subscription
    /// </summary>
    public interface ISubscriptionRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล subscription จากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสบัญชีผู้ใช้ที่ต้องการขอข้อมูล</param>
        IEnumerable<Subscription> GetSubscriptionsByUserProfileId(string userprofileId);

        #endregion Methods
    }
}
