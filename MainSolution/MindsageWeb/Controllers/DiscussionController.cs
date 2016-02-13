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
    public class DiscussionController : ApiController
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private ICommentRepository _commentRepo;
        private IUserActivityRepository _userActivityRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize discussion controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public DiscussionController(IClassCalendarRepository classCalendarRepo,
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

        // POST: api/Discussion
        public void Post(PostNewDiscussionRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.CommentId)
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

            var selectedComment = _commentRepo.GetCommentById(body.CommentId);
            if (selectedComment == null) return;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
            if (selectedUserActivity == null) return;

            var selectedLesson = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId == body.LessonId);
            if (selectedLesson == null) return;

            var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
            if (!isCommentOwner)
            {
                var canPostNewDiscussion = checkAccessPermissionToUserProfile(selectedComment.CreatedByUserProfileId);
                if (!canPostNewDiscussion) return;
            }

            var discussions = selectedComment.Discussions.ToList();
            var newDiscussion = new Comment.Discussion
            {
                id = Guid.NewGuid().ToString(),
                CreatedByUserProfileId = body.UserProfileId,
                CreatorDisplayName = userprofile.Name,
                CreatorImageUrl = userprofile.ImageProfileUrl,
                Description = body.Description,
                CreatedDate =now,
            };
            discussions.Add(newDiscussion);
            selectedComment.Discussions = discussions;
            _commentRepo.UpsertComment(selectedComment);

            selectedLesson.ParticipationAmount++;
            _userActivityRepo.UpsertUserActivity(selectedUserActivity);
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
        private bool checkAccessPermissionToUserProfile(string userprofileId)
        {
            var selectedCommentOwnerProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            var canPostNewDiscussion = selectedCommentOwnerProfile != null
                && !selectedCommentOwnerProfile.IsPrivateAccount;
            return canPostNewDiscussion;
        }

        #endregion Methods

        //// GET: api/Discussion
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Discussion/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Discussion
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Discussion/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Discussion/5
        //public void Delete(int id)
        //{
        //}
    }
}
