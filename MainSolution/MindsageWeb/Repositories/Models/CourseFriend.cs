using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class CourseFriend
    {
        #region Properties

        public string id { get; set; }
        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public IEnumerable<string> FriendWith { get; set; }

        #endregion Properties
    }
}
