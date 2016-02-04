using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class LikeLesson
    {
        #region Properties

        public string id { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string LikedByUserProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        #endregion Properties
    }
}
