using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MindsageWeb.Controllers
{
    public class LessonController : ApiController
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private ISubscriptionRepository _subscriptionRepo;
        private IClassRoomRepository _classRoomRepo;
        private ILikeLessonRepository _likeLessonRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize lesson controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="subscriptionRepo">Subscription repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        ///<param name="likeLessonRepo">Like lesson repository</param>
        public LessonController(IClassCalendarRepository classCalendarRepo,
            ISubscriptionRepository subscriptionRepo,
            IClassRoomRepository classRoomRepo,
            ILikeLessonRepository likeLessonRepo)
        {
            _classCalendarRepo = classCalendarRepo;
            _subscriptionRepo = subscriptionRepo;
            _classRoomRepo = classRoomRepo;
            _likeLessonRepo = likeLessonRepo;
        }

        #endregion Constructors

        #region Methods

        // POST: api/Lesson
        [Route("like")]
        public void Post(LikeLessonRequest data)
        {
            var isArgumentValid = data != null
                && !string.IsNullOrEmpty(data.ClassRoomId)
                && !string.IsNullOrEmpty(data.LessonId)
                && !string.IsNullOrEmpty(data.UserProfileId);
            if (!isArgumentValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(data.UserProfileId, data.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = DateTime.Now;
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(data.ClassRoomId, data.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedClass = _classRoomRepo.GetClassRoomById(data.ClassRoomId);
            var isLikeConditionValid = selectedClass != null && selectedClass.Lessons.Any(it => it.id == data.LessonId);
            if (!isLikeConditionValid) return;

            var likeLessons = _likeLessonRepo.GetLikeLessonsByLessonId(data.LessonId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();
            if (likeLessons == null) return;

            var likedLessonsByUser = likeLessons
                .Where(it => it.LikedByUserProfileId.Equals(data.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var isUnlike = likedLessonsByUser.Any();
            if (isUnlike)
            {
                foreach (var item in likedLessonsByUser)
                {
                    item.DeletedDate = now;
                    _likeLessonRepo.UpsertLikeLesson(item);
                }
            }
            else
            {
                var newLike = new LikeLesson
                {
                    id = Guid.NewGuid().ToString(),
                    ClassRoomId = data.ClassRoomId,
                    LessonId = data.LessonId,
                    LikedByUserProfileId = data.UserProfileId,
                    CreatedDate = now
                };
                likeLessons.Add(newLike);
                _likeLessonRepo.UpsertLikeLesson(newLike);
            }

            var selectedLesson = selectedClass.Lessons.First(it => it.id == data.LessonId);
            selectedLesson.TotalLikes = likeLessons.Where(it => !it.DeletedDate.HasValue).Count();
            _classRoomRepo.UpdateClassRoom(selectedClass);
        }

        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var canAccessToTheClass = _subscriptionRepo
                .GetSubscriptionsByUserProfileId(userprofileId)
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase))
                .Any();

            return canAccessToTheClass;
        }
        private bool checkAccessPermissionToSelectedClassLesson(string classRoomId, string lessonId, DateTime currentTime)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(classRoomId) && !string.IsNullOrEmpty(lessonId);
            if (!areArgumentsValid) return false;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            if (selectedClassCalendar == null) return false;

            var canAccessToTheLesson = selectedClassCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LessonId.Equals(lessonId, StringComparison.CurrentCultureIgnoreCase))
                .Where(it => it.BeginDate <= currentTime)
                .Any();
            return canAccessToTheLesson;
        }

        #endregion Methods

        //// GET: api/Lesson
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Lesson/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Lesson
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Lesson/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Lesson/5
        //public void Delete(int id)
        //{
        //}
    }
}
