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
        private ILessonCatalogRepository _lessonCatalogRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize lesson controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="subscriptionRepo">Subscription repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        ///<param name="likeLessonRepo">Like lesson repository</param>
        ///<param name="lessonCatalogRepo">Lesson catalog repository</param>
        public LessonController(IClassCalendarRepository classCalendarRepo,
            ISubscriptionRepository subscriptionRepo,
            IClassRoomRepository classRoomRepo,
            ILikeLessonRepository likeLessonRepo,
            ILessonCatalogRepository lessonCatalogRepo)
        {
            _classCalendarRepo = classCalendarRepo;
            _subscriptionRepo = subscriptionRepo;
            _classRoomRepo = classRoomRepo;
            _likeLessonRepo = likeLessonRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
        }

        #endregion Constructors

        #region Methods

        // GET: api/lesson/{lesson-id}/{class-room-id}/{user-id}
        [Route(":id/:classRoomId/:userId")]
        public LessonContentRespond Get(string id, string classRoomId, string userId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return null;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(userId, classRoomId);
            if (!canAccessToTheClassRoom) return null;

            var now = DateTime.Now;
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(classRoomId, id, now);
            if (!canAccessToTheClassLesson) return null;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            if (selectedClassRoom == null) return null;

            var selectedLesson = selectedClassRoom.Lessons.FirstOrDefault(it => it.id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if(selectedLesson == null) return null;

            var selectedLessonCatalog = _lessonCatalogRepo.GetLessonCatalogById(selectedLesson.LessonCatalogId);
            if (selectedLessonCatalog == null) return null;

            return new LessonContentRespond
            {
                Advertisments = selectedLessonCatalog.Advertisments,
                CourseCatalogId = selectedLessonCatalog.CourseCatalogId,
                CreatedDate = selectedLessonCatalog.CreatedDate,
                ExtraContentUrls = selectedLessonCatalog.ExtraContentUrls,
                FullDescription = selectedLessonCatalog.FullDescription,
                FullTeacherLessonPlan = selectedLessonCatalog.FullTeacherLessonPlan,
                id = id,
                Order = selectedLessonCatalog.Order,
                PrimaryContentURL = selectedLessonCatalog.PrimaryContentURL,
                SemesterName = selectedLessonCatalog.SemesterName,
                ShortDescription = selectedLessonCatalog.ShortDescription,
                ShortTeacherLessonPlan = selectedLessonCatalog.ShortTeacherLessonPlan,
                Title = selectedLessonCatalog.Title,
                UnitNo = selectedLessonCatalog.UnitNo,
                //CourseMessage = // TODO: Display course message & check show/hide
                IsTeacher = true, // HACK: Check role
                TotalLikes = selectedLesson.TotalLikes
            };
        }

        // POST: api/lesson/like
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

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(data.ClassRoomId);
            var isLikeConditionValid = selectedClassRoom != null && selectedClassRoom.Lessons.Any(it => it.id == data.LessonId);
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

            var selectedLesson = selectedClassRoom.Lessons.First(it => it.id == data.LessonId);
            selectedLesson.TotalLikes = likeLessons.Where(it => !it.DeletedDate.HasValue).Count();
            _classRoomRepo.UpdateClassRoom(selectedClassRoom);

            // TODO: อัพเดท % lesson progress ของผู้ใช้
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
