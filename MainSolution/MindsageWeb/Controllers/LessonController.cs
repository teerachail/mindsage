﻿using MindsageWeb.Repositories;
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
        private IUserProfileRepository _userprofileRepo;
        private IClassRoomRepository _classRoomRepo;
        private ILikeLessonRepository _likeLessonRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private ICommentRepository _commentRepo;
        private IFriendRequestRepository _friendRequestRepo;
        private IUserActivityRepository _userActivityRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize lesson controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="likeLessonRepo">Like lesson repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="friendRequestRepo">Friend request repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public LessonController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IClassRoomRepository classRoomRepo,
            ILikeLessonRepository likeLessonRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            ICommentRepository commentRepo,
            IFriendRequestRepository friendRequestRepo,
            IUserActivityRepository userActivityRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _classRoomRepo = classRoomRepo;
            _likeLessonRepo = likeLessonRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _commentRepo = commentRepo;
            _friendRequestRepo = friendRequestRepo;
            _userActivityRepo = userActivityRepo;
            _dateTime = dateTime;
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

            UserProfile.Subscription subscription;
            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(userId, classRoomId, out subscription);
            if (!canAccessToTheClassRoom) return null;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(classRoomId, id, now);
            if (!canAccessToTheClassLesson) return null;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            if (selectedClassRoom == null) return null;

            var selectedLesson = selectedClassRoom.Lessons.FirstOrDefault(it => it.id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if (selectedLesson == null) return null;

            var selectedLessonCatalog = _lessonCatalogRepo.GetLessonCatalogById(selectedLesson.LessonCatalogId);
            if (selectedLessonCatalog == null) return null;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(userId, classRoomId);
            if (selectedUserActivity == null) return null;
            var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if (selectedLessonActivity == null) return null;
            var shouldUpdateSawPrimaryContent = !selectedLessonActivity.SawContentIds.Contains(selectedLessonCatalog.PrimaryContentURL);
            if (shouldUpdateSawPrimaryContent)
            {
                var sawList = selectedLessonActivity.SawContentIds.ToList();
                sawList.Add(selectedLessonCatalog.PrimaryContentURL);
                selectedLessonActivity.SawContentIds = sawList;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);
            }

            var isTeacher = subscription.Role == UserProfile.AccountRole.Teacher;

            return new LessonContentRespond
            {
                Advertisments = selectedLessonCatalog.Advertisments,
                CourseCatalogId = selectedLessonCatalog.CourseCatalogId,
                CreatedDate = selectedLessonCatalog.CreatedDate,
                ExtraContentUrls = selectedLessonCatalog.ExtraContentUrls,
                FullDescription = selectedLessonCatalog.FullDescription,
                FullTeacherLessonPlan = isTeacher ? selectedLessonCatalog.FullTeacherLessonPlan : string.Empty,
                id = id,
                Order = selectedLessonCatalog.Order,
                PrimaryContentURL = selectedLessonCatalog.PrimaryContentURL,
                SemesterName = selectedLessonCatalog.SemesterName,
                ShortDescription = selectedLessonCatalog.ShortDescription,
                ShortTeacherLessonPlan = isTeacher ? selectedLessonCatalog.ShortTeacherLessonPlan : string.Empty,
                Title = selectedLessonCatalog.Title,
                UnitNo = selectedLessonCatalog.UnitNo,
                CourseMessage = selectedClassRoom.Message,
                IsTeacher = isTeacher,
                TotalLikes = selectedLesson.TotalLikes
            };
        }

        // GET: api/lesson/{lesson-id}/{class-room-id}/comments/{user-id}
        [Route(":id/:classRoomId/comments/:userId")]
        public IEnumerable<Comment> Comments(string id, string classRoomId, string userId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return null;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(userId, classRoomId);
            if (!canAccessToTheClassRoom) return null;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(classRoomId, id, now);
            if (!canAccessToTheClassLesson) return null;

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(userId);
            if (friendRequests == null) return null;

            var friendIds = friendRequests
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Status == FriendRequest.RelationStatus.Friend)
                .Select(it => it.ToUserProfileId);

            var filterByCreatorNames = friendIds.Union(new string[] { userId });
            var comments = _commentRepo.GetCommentsByLessonId(id, filterByCreatorNames)
                .Where(it => !it.DeletedDate.HasValue)
                .OrderByDescending(it => it.CreatedDate)
                .ToList();
            comments.ForEach(comment =>
            {
                comment.Discussions = comment.Discussions
                .Where(it => !it.DeletedDate.HasValue)
                .OrderByDescending(it => it.CreatedDate);
            });
            return comments;
        }

        // POST: api/lesson/like
        [Route("like")]
        public void Post(LikeLessonRequest body)
        {
            var isArgumentValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!isArgumentValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
            var isLikeConditionValid = selectedClassRoom != null && selectedClassRoom.Lessons.Any(it => it.id == body.LessonId);
            if (!isLikeConditionValid) return;

            var likeLessons = _likeLessonRepo.GetLikeLessonsByLessonId(body.LessonId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();
            if (likeLessons == null) return;

            var likedLessonsByThisUser = likeLessons
                .Where(it => it.LikedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var isUnlike = likedLessonsByThisUser.Any();
            if (isUnlike)
            {
                foreach (var item in likedLessonsByThisUser)
                {
                    item.DeletedDate = now;
                    _likeLessonRepo.UpsertLikeLesson(item);
                }
            }
            else
            {
                var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
                if (selectedUserActivity == null) return;
                var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(body.LessonId));
                if (selectedLessonActivity == null) return;
                selectedLessonActivity.ParticipationAmount++;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);

                var newLike = new LikeLesson
                {
                    id = Guid.NewGuid().ToString(),
                    ClassRoomId = body.ClassRoomId,
                    LessonId = body.LessonId,
                    LikedByUserProfileId = body.UserProfileId,
                    CreatedDate = now
                };
                likeLessons.Add(newLike);
                _likeLessonRepo.UpsertLikeLesson(newLike);
            }

            var selectedLesson = selectedClassRoom.Lessons.First(it => it.id == body.LessonId);
            selectedLesson.TotalLikes = likeLessons.Where(it => !it.DeletedDate.HasValue).Count();
            _classRoomRepo.UpdateClassRoom(selectedClassRoom);
        }

        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId)
        {
            UserProfile.Subscription subscription;
            return checkAccessPermissionToSelectedClassRoom(userprofileId, classRoomId, out subscription);
        }
        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId, out UserProfile.Subscription subscription)
        {
            subscription = null;
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            if (selectedUserProfile == null) return false;

            subscription = selectedUserProfile
                .Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            var canAccessToTheClass = subscription != null;
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
