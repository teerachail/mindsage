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

        private IClassRoomRepository _classRoomRepo;
        private ILikeLessonRepository _likeLessonRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize lesson controller
        /// </summary>
        /// <param name="classRoomRepo">Class room repository</param>
        ///<param name="likeLessonRepo">Like lesson repository</param>
        public LessonController(IClassRoomRepository classRoomRepo, ILikeLessonRepository likeLessonRepo)
        {
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
                && !string.IsNullOrEmpty(data.CourseId)
                && !string.IsNullOrEmpty(data.LessonId)
                && !string.IsNullOrEmpty(data.UserProfileId);
            if (!isArgumentValid) return;

            var selectedClass = _classRoomRepo.GetClassRoomById(data.CourseId);
            var isLikeConditionValid = selectedClass != null && selectedClass.Lessons.Any(it => it.id == data.LessonId);
            // TODO: ตรวจสอบสิทธิ์ในการเข้าถึง course 
            // TODO: ตรวจสอบสิทธิ์ในการเข้าถึง lesson
            if (!isLikeConditionValid) return;

            var likeLessons = _likeLessonRepo.GetLikeLessonsByLessonId(data.LessonId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();
            if (likeLessons == null) return;

            var likedLessonsByUser = likeLessons
                .Where(it => it.LikedByUserProfileId.Equals(data.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var now = DateTime.Now;
            var isCancel = likedLessonsByUser.Any();
            if (isCancel)
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
                    ClassRoomId = data.CourseId,
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
