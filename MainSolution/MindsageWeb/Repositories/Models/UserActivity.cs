using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class UserActivity
    {
        #region Properties

        public string id { get; set; }
        public string UserProfileName { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<LessonActivity> LessonActivities { get; set; }

        #endregion Properties

        public class LessonActivity
        {
            #region Properties

            public string id { get; set; }
            public string LessonId { get; set; }
            public int ViewContentsCounts { get; set; }
            public int TotalContents { get; set; }
            public int CreatedComments { get; set; }
            public int SendLikes { get; set; }

            #endregion Properties
        }
    }
}
