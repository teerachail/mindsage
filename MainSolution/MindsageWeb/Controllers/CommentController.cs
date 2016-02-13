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
    public class CommentController : ApiController
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private ICommentRepository _commentRepo;
        private IUserActivityRepository _userActivityRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize comment controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public CommentController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            ICommentRepository commentRepo,
            IUserActivityRepository userActivityRepo)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _commentRepo = commentRepo;
            _userActivityRepo = userActivityRepo;
        }

        #endregion Constructors

        #region Methods

        // POST: api/comment
        public void Post(PostNewCommentRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.Description)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var now = DateTime.Now;
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
            if (selectedUserActivity == null) return;

            var selectedLesson = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId == body.LessonId);
            if (selectedLesson == null) return;

            var newComment = new Comment
            {
                id = Guid.NewGuid().ToString(),
                ClassRoomId = body.ClassRoomId,
                CreatedByUserProfileId = body.UserProfileId,
                Description = body.Description,
                LessonId = body.LessonId,
                CreatedDate = now,
                CreatorDisplayName = userprofile.Name,
                CreatorImageUrl = userprofile.ImageProfileUrl,
                Discussions = Enumerable.Empty<Comment.Discussion>(),
            };
            _commentRepo.UpsertComment(newComment);

            selectedLesson.CreatedCommentAmount++;
            _userActivityRepo.UpsertUserActivity(selectedUserActivity);
        }

        // PUT: api/comment/{comment-id}
        public void Put(string id, RemoveCommentRequest body)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = DateTime.Now;
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(id);
            if (selectedComment == null) return;

            var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
            if (!isCommentOwner) return;

            selectedComment.DeletedDate = now;
            _commentRepo.UpsertComment(selectedComment);
        }

        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId)
        {
            UserProfile userprofile;
            return checkAccessPermissionToSelectedClassRoom(userprofileId, classRoomId, out userprofile);
        }
        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId, out UserProfile userprofile)
        {
            userprofile = null;
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            if (selectedUserProfile == null) return false;
            userprofile = selectedUserProfile;

            var canAccessToTheClass = selectedUserProfile
                .Subscriptions
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

        //// GET: api/Comment
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Comment/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Comment
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Comment/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Comment/5
        //public void Delete(int id)
        //{
        //}
    }
}
